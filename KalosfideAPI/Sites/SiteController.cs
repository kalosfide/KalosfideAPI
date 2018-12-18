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

        public SiteController(ISiteService service) : base(service)
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

        protected override Task<bool> EditeEstPermis(CarteUtilisateur carte, KeyParam param)
        {
            return Task.FromResult(carte.EstAdministrateur || carte.EstPropriétaire(param));
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

        [HttpGet("/api/site/nomPris/{nomSite}")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(404)] // Not found
        [AllowAnonymous]
        public async Task<IActionResult> NomPris(string nomSite)
        {
            return Ok(await _service.NomPris(nomSite));
        }

        [HttpGet("/api/site/nomPrisParAutre/{nomSite}")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(404)] // Not found
        [AllowAnonymous]
        public async Task<IActionResult> NomPrisParAutre([FromQuery] KeyUidRno key, string nomSite)
        {
            return Ok(await _service.NomPrisParAutre(key, nomSite));
        }

        [HttpGet("/api/site/titrePris/{titre}")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(404)] // Not found
        [AllowAnonymous]
        public async Task<IActionResult> TitrePris(string titre)
        {
            return Ok(await _service.TitrePris(titre));
        }

        [HttpGet("/api/site/titrePrisParAutre/{titre}")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(404)] // Not found
        [AllowAnonymous]
        public async Task<IActionResult> TitrePrisParAutre([FromQuery] KeyUidRno key, string titre)
        {
            return Ok(await _service.TitrePrisParAutre(key, titre));
        }

    }
}
