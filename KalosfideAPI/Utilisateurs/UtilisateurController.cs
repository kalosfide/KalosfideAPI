using KalosfideAPI.Data;
using KalosfideAPI.Erreurs;
using KalosfideAPI.Partages;
using KalosfideAPI.Sécurité;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KalosfideAPI.Utilisateurs
{
    [Route("api/[controller]")]
    [ApiValidationFilter]
    [Authorize]
    public class UtilisateurController : SaveChangesController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtFabrique _jwtFactory;
        private readonly JwtFabriqueOptions _jwtOptions;
        private readonly IAuthorizationService _autorisation;

        private readonly IUtilisateurTransformation _transformation;
        private readonly IUtilisateurService _service;

        public UtilisateurController(
            UserManager<ApplicationUser> userManager,
            IJwtFabrique jwtFactory,
            IOptions<JwtFabriqueOptions> jwtOptions,
            IAuthorizationService autorisation,
            IUtilisateurService service,
            IUtilisateurTransformation transformation
        )
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            _autorisation = autorisation;
            _service = service;
            _transformation = transformation;
        }

        private ErreurDeModel ErreurDoublon(string code, string texte, string doublon)
        {
            return new ErreurDeModel
            {
                Code = "Doublon_" + code,
                Description = $"{texte}: {doublon} est déjà associé à un compte"
            };
        }
         
        private async Task<ErreurDeModel> NomExisteDéjà(ApplicationUser applicationUser)
        {
            ApplicationUser existant = await _userManager.FindByNameAsync(applicationUser.UserName);
            if (existant != null && existant.Id != applicationUser.Id)
            {
                return ErreurDoublon("Nom", "Nom", applicationUser.UserName);
            }
            return null;
        }
         
        private async Task<ErreurDeModel> EmailExisteDéjà(ApplicationUser applicationUser)
        {
            ApplicationUser existant = await _userManager.FindByNameAsync(applicationUser.Email);
            if (existant != null && existant.Id != applicationUser.Id)
            {
                return ErreurDoublon("Email", "Email", applicationUser.Email);
            }
            return null;
        }

        // POST api/utilisateur/enregistre
        [HttpPost("/api/utilisateur/enregistre")]
        [ProducesResponseType(201)] // created
        [ProducesResponseType(400)] // Bad request
        [AllowAnonymous]
        public async Task<IActionResult> Enregistre([FromBody]EnregistrementVue vue)
        {

            ApplicationUser applicationUser = new ApplicationUser
            {
                UserName = vue.Nom,
                Email = vue.Email,
            };

            ErreurDeModel erreur = await NomExisteDéjà(applicationUser);
            if (erreur == null)
            {
                erreur = await EmailExisteDéjà(applicationUser);
            }
            if (erreur != null)
            {
                erreur.AjouteAModelState(ModelState);
                return BadRequest(ModelState);
            }

            BaseServiceRetour<Utilisateur> retour = await _service.Enregistre(applicationUser, vue.Password);

            if (retour.Ok)
            {
                Utilisateur utilisateur = retour.Entité;
                return CreatedAtAction(nameof(Lit), new { id = utilisateur.UtilisateurId }, _transformation.CréeVue(utilisateur));
            }

            return SaveChangesActionResult(retour);
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
        public async Task<IActionResult> LitTout()
        {
            return Ok(await _service.Lit());
        }

        // PUT api/utilisateur
        [HttpPut]
        [ProducesResponseType(204)] // no content
        [ProducesResponseType(400)] // Bad request
        [ProducesResponseType(404)] // Not found
        public async Task<IActionResult> Edite(UtilisateurVue vue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Utilisateur utilisateur = _transformation.CréeDonnée(vue);

            utilisateur = await _service.Lit(vue.UtilisateurId);

            if (utilisateur == null)
            {
                return NotFound();
            }

            _transformation.CopieVueDansDonnées(utilisateur, vue);

            var retour = await _service.Update(utilisateur);

            return SaveChangesActionResult(retour);
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
            var retour = await _service.Delete(utilisateur);

            return SaveChangesActionResult(retour);
        }

        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(404)] // Not found
        [ProducesResponseType(400)] // Bad request
        public async Task<IActionResult> ChangeRoleActif(int roleNo )
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

            Utilisateur utilisateur = await _service.UtilisateurAvecListeRoles(user);

            Role role = null;
            foreach (Role r in utilisateur.Roles)
            {
                if (r.No == roleNo)
                {
                    role = r;
                    break;
                }
            }

            if (role == null)
            {
                return NotFound();
            }

            var retour = await _service.ChangeRoleActif(utilisateur, role);
            if (retour.Ok)
            {
                Utilisateur utilisateurAvecRoleSelectionné = await _service.UtilisateurAvecRoleSelectionné(user);
                JwtRéponse jwtRéponse = _jwtFactory.CréeReponse(user, utilisateurAvecRoleSelectionné);
                return new OkObjectResult(jwtRéponse);
            }
            return SaveChangesActionResult(retour);
        }

        [AllowAnonymous]
        [HttpPost("Connecte")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(400)] // Bad request
        public async Task<IActionResult> Connecte([FromBody]ConnectionVue connection)
        {
            ApplicationUser user = await ApplicationUserVérifié(connection.UserName, connection.Password);
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
            Utilisateur utilisateurAvecRoleSelectionné = await _service.UtilisateurAvecRoleSelectionné(user);
            JwtRéponse jwtRéponse = _jwtFactory.CréeReponse(user, utilisateurAvecRoleSelectionné);
            return new OkObjectResult(jwtRéponse);
        }

        private async Task<ApplicationUser> ApplicationUserVérifié(string userName, string password)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                // get the user to verifty
                ApplicationUser userToVerify = await _userManager.FindByNameAsync(userName);

                if (userToVerify != null)
                {
                    // check the credentials
                    if (await _userManager.CheckPasswordAsync(userToVerify, password))
                    {
                        return await Task.FromResult(userToVerify);
                    }
                }
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ApplicationUser>(null);
        }
    }
}
