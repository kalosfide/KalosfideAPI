using KalosfideAPI.Data;
using KalosfideAPI.Erreurs;
using KalosfideAPI.Partages;
using KalosfideAPI.Sécurité;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Roles
{

    [Route("api/[controller]/[action]/{param?}")]
    [ApiController]
    [ApiValidationFilter]
    [Authorize]
    public class RoleController : KeyUtilisateurIdNoController<Role, RoleVue>
    {
        public RoleController(
            IAuthorizationService autorisation,
            IRoleService service,
            IRoleTransformation transformation
        ) : base(autorisation, service, transformation)
        {
        }

        private IRoleService _service { get => __service as IRoleService; }
        private IRoleTransformation _transformation { get => __transformation as IRoleTransformation; }

        // POST api/utilisateur/ajoute
        [HttpPost("/api/utilisateur/enregistre")]
        [ProducesResponseType(201)] // created
        [ProducesResponseType(400)] // Bad request
        public new async Task<IActionResult> Ajoute(RoleVue vue)
        {
            return await base.Ajoute(vue);
        }

        // GET api/utilisateur/?id
        [HttpGet]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(404)] // Not found
        public new async Task<IActionResult> Lit(string param)
        {
            return await base.Lit(param);
        }

        public new async Task<IActionResult> Edite(RoleVue vue)
        {
            return await base.Edite(vue);
        }

        public new async Task<IActionResult> Supprime(string param)
        {
            return await base.Supprime(param);
        }

        public new async Task<IActionResult> DernierNo(string param)
        {
            return await base.DernierNo(param);
        }

        public async Task<IActionResult> Accepte(string param)
        {
            var ikey = KeyUtilisateurIdNoFabrique.CréeKey(param);
            if (ikey == null || ikey is KeyUtilisateurId)
            {
                return BadRequest();
            }

            var donnée = await __service.Lit(ikey as KeyUtilisateurIdNo);
            if (donnée == null)
            {
                return NotFound();
            }
            OperationAuthorizationRequirement[] requirements = new OperationAuthorizationRequirement[]
                { RoleActions.Accepte.Requirement };
            var permis = await _autorisation.AuthorizeAsync(HttpContext.User, donnée, requirements);
            if (!permis.Succeeded)
            {
                return Forbid();
            }
            await _service.ChangeEtat(donnée, EtatRole.Actif);
            return Ok(donnée);
        }
    }
}