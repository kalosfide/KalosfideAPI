using KalosfideAPI.Data;
using KalosfideAPI.Erreurs;
using KalosfideAPI.Partages;
using KalosfideAPI.Sécurité;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KalosfideAPI.Utilisateurs
{
    [Route("api/[controller]")]
    [ApiValidationFilter]
    [Authorize]
    public class UtilisateurController : BaseController
    {
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly IJwtFabrique _jwtFabrique;

        protected readonly IUtilisateurTransformation _transformation;
        protected readonly IUtilisateurService _service;

        public UtilisateurController(
            UserManager<ApplicationUser> userManager,
            IJwtFabrique jwtFabrique,
            IUtilisateurService service,
            IUtilisateurTransformation transformation
        )
        {
            _userManager = userManager;
            _jwtFabrique = jwtFabrique;
            _service = service;
            _transformation = transformation;
            opérations = new List<Opération>
            {
                new Opération { Nom = nameof(Lit) },
                new Opération { Nom = nameof(Edite) },
                new Opération { Nom = nameof(Supprime) },
            };
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
            ApplicationUser existant = await _userManager.FindByNameAsync(applicationUser.UserName);
            if (existant != null && existant.Id != applicationUser.Id)
            {
                return ErreurDoublon("Nom", "Nom", applicationUser.UserName);
            }
            return null;
        }
         
        protected async Task<ErreurDeModel> EmailExisteDéjà(ApplicationUser applicationUser)
        {
            ApplicationUser existant = await _userManager.FindByNameAsync(applicationUser.Email);
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
            JwtRéponse jwtRéponse = await _jwtFabrique.CréeReponse(user, utilisateurAvecRoleSelectionné);
            return new OkObjectResult(jwtRéponse);
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
        public async Task<IActionResult> Lit()
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

            var retour = await _service.Edite(utilisateur);

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
            var retour = await _service.Supprime(utilisateur);

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
                if (r.RoleNo == roleNo)
                {
                    role = r;
                    break;
                }
            }

            if (role == null)
            {
                return NotFound();
            }

            var retour = await _service.ChangeRole(utilisateur, role);
            if (retour.Ok)
            {
                Utilisateur utilisateurAvecRoleSelectionné = await _service.UtilisateurAvecRoleSelectionné(user);
                JwtRéponse jwtRéponse = await _jwtFabrique.CréeReponse(user, utilisateurAvecRoleSelectionné);
                return new OkObjectResult(jwtRéponse);
            }
            return SaveChangesActionResult(retour);
        }

        protected async Task<ApplicationUser> ApplicationUserVérifié(string userName, string password)
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
