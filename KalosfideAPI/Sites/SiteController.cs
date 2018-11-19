using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Erreurs;
using KalosfideAPI.Partages;
using KalosfideAPI.Partages.KeyParams;
using KalosfideAPI.Roles;
using KalosfideAPI.Sécurité;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Sites
{

    [ApiController]
    [Route("UidRno")]
    [ApiValidationFilter]
    [Authorize]
    public class SiteController: KeyUidRnoController<Site, SiteVue>
    {
        private ISiteService _service { get => __service as ISiteService; }

        public SiteController(
            ISiteService service,
            ISiteTransformation transformation
        ) : base(service, transformation)
        {
        }

        [HttpGet("/api/site/lit/{uid?}/{rno?}")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(404)] // Not found
        public async Task<IActionResult> Lit([FromQuery] KeyUidRno param)
        {
            return await base.Lit(param.KeyParam);
        }

        protected override bool ListeEstPermis(CarteUtilisateur carte)
        {
            return true;
        }
        [HttpGet("/api/site/liste")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(404)] // Not found
        [AllowAnonymous]
        public new async Task<IActionResult> Liste()
        {
            return await base.Liste();
        }

        protected override bool EditeEstPermis(CarteUtilisateur carte, KeyParam param)
        {
            return carte.EstAdministrateur || carte.EstPropriétaire(param);
        }
        [HttpPut("/api/site/edite/{uid?}/{rno?}")]
        public new async Task<IActionResult> Edite(SiteVue vue)
        {
            return await base.Edite(vue);
        }

        [HttpGet("/api/site/trouveParNom/{nomSite}")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(404)] // Not found
        [AllowAnonymous]
        public async Task<IActionResult> TrouveParNom(string nomSite)
        {
            Site site = await _service.TrouveParNom(nomSite);
            if (site == null)
            {
                return NotFound();
            }
            return Ok(site);
        }
    }
}
