using KalosfideAPI.Data;
using KalosfideAPI.Erreurs;
using KalosfideAPI.Partages;
using KalosfideAPI.Sécurité;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace KalosfideAPI.Utilisateurs
{
    [Route("api/[controller]")]
    [ApiValidationFilter]
    [Authorize]
    public class UtilisateurController : BaseController
    {
        protected readonly IJwtFabrique _jwtFabrique;

        protected readonly IUtilisateurService _service;

        public UtilisateurController(
            IJwtFabrique jwtFabrique,
            IUtilisateurService service
        )
        {
            _jwtFabrique = jwtFabrique;
            _service = service;
        }

        protected ErreurDeModel ErreurDoublon(string code, string texte, string doublon)
        {
            return new ErreurDeModel
            {
                Code = "Doublon_" + code,
                Description = $"{texte}: {doublon} est déjà associé à un compte"
            };
        }
         
        protected async Task<ErreurDeModel> NomExisteDéjà(ApplicationUser applicationUser)
        {
            ApplicationUser existant = await _service.TrouveParNom(applicationUser.UserName);
            if (existant != null && existant.Id != applicationUser.Id)
            {
                return ErreurDoublon("Nom", "Nom", applicationUser.UserName);
            }
            return null;
        }
         
        protected async Task<ErreurDeModel> EmailExisteDéjà(ApplicationUser applicationUser)
        {
            ApplicationUser existant = await _service.TrouveParEmail(applicationUser.Email);
            if (existant != null && existant.Id != applicationUser.Id)
            {
                return ErreurDoublon("Email", "Email", applicationUser.Email);
            }
            return null;
        }

        [HttpPost("Connecte")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(400)] // Bad request
        [AllowAnonymous]
        public async Task<IActionResult> Connecte([FromBody]ConnectionVue connection)
        {
            ApplicationUser user = await _service.ApplicationUserVérifié(connection.UserName, connection.Password);
            if (user == null)
            {
                ErreurDeModel erreur = new ErreurDeModel
                {
                    Code = "identifiants",
                    Description = "Nom ou mot de passe invalide"
                };
                erreur.AjouteAModelState(ModelState);
                return BadRequest(ModelState);
            }
            
            return await Connecte(user, connection.Persistant);
        }

        protected async Task<IActionResult> Connecte(ApplicationUser user, bool persistant)
        {
            CarteUtilisateur carteUtilisateur = await _service.CréeCarteUtilisateur(user);

            if (!carteUtilisateur.EstUtilisateurActif)
            {
                ErreurDeModel erreur = new ErreurDeModel
                {
                    Code = "etatUtilisateur",
                    Description = "Cet utilisateur n'est pas autorisé"
                };
                return StatusCode(403, erreur);
            }

            JwtRéponse jwtRéponse = await _jwtFabrique.CréeReponse(carteUtilisateur);
            Request.HttpContext.Response.Headers.Add(JwtFabrique.NomJwtRéponse, JsonConvert.SerializeObject(jwtRéponse));
            await _service.Connecte(user, persistant);
            return new OkObjectResult(carteUtilisateur);
        }

        [HttpPost("deconnecte")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(400)] // Bad request
        public async Task<IActionResult> Deconnecte()
        {
            await _service.Déconnecte();
            
            return Ok();
        }

        // GET api/utilisateur/?id
        [HttpGet("{id}")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(404)] // Not found
        public async Task<IActionResult> Lit(string id)
        {
            Utilisateur utilisateur = await _service.Lit(id);
            if (utilisateur == null)
            {
                return NotFound();
            }
            return Ok(utilisateur);
        }

        // GET api/utilisateur
        [HttpGet]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(404)] // Not found
        public async Task<IActionResult> Liste()
        {
            return Ok(await _service.Lit());
        }

        // DELETE api/utilisateur/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)] // no content
        [ProducesResponseType(404)] // Not found
        [ProducesResponseType(500)] // 500 Internal Server Error
        public async Task<IActionResult> Supprime(string id)
        {
            var utilisateur = await _service.Lit(id);
            if (utilisateur == null)
            {
                return NotFound();
            }
            var retour = await _service.Supprime(utilisateur);

            return SaveChangesActionResult(retour);
        }
    }
}
