using KalosfideAPI.Data;
using KalosfideAPI.Data.Enums;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Erreurs;
using KalosfideAPI.Partages.KeyString;
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
    public class RoleController : KeyUIdRNoController<Role, RoleVue>
    {
        public RoleController(
            IRoleService service,
            IRoleTransformation transformation
        ) : base(service, transformation)
        {
        }

        private new IRoleService _service { get => __service as IRoleService; }
        private new IRoleTransformation _transformation { get => __transformation as IRoleTransformation; }

        // POST api/utilisateur/ajoute
        [HttpPost]
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

        [HttpGet]
        public new async Task<IActionResult> DernierNo(string param)
        {
            return await base.DernierNo(param);
        }

        private async Task<IActionResult> ChangeEtat(string key, string état, Func<Role, bool> peutChanger)
        {
            AKeyString aKey = CréeAKey(key);
            if (aKey == null)
            {
                return BadRequest();
            }

            var donnée = await _service.Lit(aKey);
            if (donnée == null)
            {
                return NotFound();
            }

            bool permis = peutChanger(donnée);
            if (!permis)
            {
                return Forbid();
            }
            var retour = await _service.ChangeEtat(donnée, EtatRole.Actif);

            return SaveChangesActionResult(retour);
        }

        private bool PeutChangerEtat(Role role)
        {
            var revendications = Sécurité.RevendicationsFabrique.Revendications(HttpContext.User);
            if (role.Type == TypeDeRole.Fournisseur.Code)
            {
                return revendications.EstAdministrateur;
            }
            if (role.Type == TypeDeRole.Client.Code)
            {
                return revendications.UtilisateurId == role.FournisseurId && revendications.EtatUtilisateur == EtatUtilisateur.Actif
                        && revendications.EtatRole == EtatRole.Actif;
            }
            return false;
        }

        public async Task<IActionResult> Accepte(string key)
        {
            return await ChangeEtat(key, EtatRole.Actif, PeutChangerEtat);
        }

        public async Task<IActionResult> Refuse(string key)
        {
            return await ChangeEtat(key, EtatRole.Inactif, PeutChangerEtat);
        }

        public async Task<IActionResult> Fournisseurs()
        {
            var donnée = await _service.Fournisseurs();
            return Ok(donnée);
        }
    }
}