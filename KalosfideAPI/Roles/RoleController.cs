using KalosfideAPI.Data;
using KalosfideAPI.Data.Constantes;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Erreurs;
using KalosfideAPI.Partages;
using KalosfideAPI.Partages.KeyParams;
using KalosfideAPI.Sécurité;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace KalosfideAPI.Roles
{

    [Route("api/[controller]/[action]/{param?}")]
    [ApiController]
    [ApiValidationFilter]
    [Authorize]
    public class RoleController : BaseController
    {
        private IRoleService _service;

        public RoleController(IRoleService service)
        {
            _service = service;
        }

        [HttpPost]
        [ProducesResponseType(200)] // ok
        [ProducesResponseType(400)] // Bad request
        [ProducesResponseType(403)] // forbidden
        [ProducesResponseType(404)] // not found
        public async Task<IActionResult> ChangeEtat([FromQuery] KeyUidRno param, [FromBody] string etat)
        {
            if (!TypeEtatRole.EstValide(etat))
            {
                return BadRequest();
            }

            var donnée = await _service.Lit(param.KeyParam);
            if (donnée == null)
            {
                return NotFound();
            }

            bool permis = ChangeEtatEstPermis(donnée);
            if (!permis)
            {
                return StatusCode(403);
            }
            var retour = await _service.ChangeEtat(donnée, etat);

            return SaveChangesActionResult(retour);
        }

        private bool ChangeEtatEstPermis(Role role)
        {
            CarteUtilisateur carte = new CarteUtilisateur();
            carte.PrendClaims(HttpContext.User);
            if (role.EstFournisseur)
            {
                return carte.EstAdministrateur;
            }
            if (role.EstClient)
            {
                return carte.EstPropriétaire(role.SiteParam);
            }
            return false;
        }
    }
}