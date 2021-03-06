﻿using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Partages.KeyParams;
using KalosfideAPI.Sécurité;
using KalosfideAPI.Utilisateurs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KalosfideAPI.Fournisseurs
{

    [ApiController]
    [Route("UidRno")]
    [Authorize]
    public class FournisseurController: KeyUidRnoController<Fournisseur, FournisseurVue>
    {
        public FournisseurController(IFournisseurService service, IUtilisateurService utilisateurService) : base(service, utilisateurService)
        {
        }

        private IFournisseurService _service { get => __service as IFournisseurService; }

        protected override bool LitEstPermis(CarteUtilisateur carte, KeyParam param)
        {
            return carte.EstAdministrateur || carte.EstPropriétaire(param);
        }
        [HttpGet]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(404)] // Not found
        public async Task<IActionResult> Lit([FromQuery] KeyUidRno param)
        {
            return await base.Lit(param.KeyParam);
        }

        public new async Task<IActionResult> Edite(FournisseurVue vue)
        {
            return await base.Edite(vue);
        }

        public  async Task<IActionResult> Supprime([FromQuery] KeyUidRno param)
        {
            return await base.Supprime(param.KeyParam);
        }
        
    }
}
