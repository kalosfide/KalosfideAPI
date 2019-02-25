using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Erreurs;
using KalosfideAPI.Partages;
using KalosfideAPI.Partages.KeyParams;
using KalosfideAPI.Roles;
using KalosfideAPI.Sécurité;
using KalosfideAPI.Utilisateurs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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

        public SiteController(ISiteService service, IUtilisateurService utilisateurService) : base(service, utilisateurService)
        {
            dEditeEstPermis = EditeEstPermis;
        }

        [HttpGet("/api/site/lit")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(403)] // Forbid
        [ProducesResponseType(404)] // Not found
        public async Task<IActionResult> Lit([FromQuery] KeyUidRno param)
        {
            return await base.Lit(param.KeyParam);
        }

        [HttpPost("/api/site/ouvre")]
        [ProducesResponseType(201)] // created
        [ProducesResponseType(403)] // Forbid
        [ProducesResponseType(400)] // Bad request
        public async Task<IActionResult> Ouvre([FromQuery] KeyUidRno key)
        {
            CarteUtilisateur carte = await _utilisateurService.CréeCarteUtilisateur(HttpContext.User);
            if (carte == null)
            {
                // fausse carte
                return Forbid();
            }

            if (!carte.EstPropriétaire(key.KeyParam))
            {
                return Forbid();
            }

            RetourDeService retour = await _service.Ouvre(key);
            if (retour == null)
            {
                return BadRequest();
            }

            return SaveChangesActionResult(retour);
        }

        [HttpPost("/api/site/ferme")]
        [ProducesResponseType(201)] // created
        [ProducesResponseType(400)] // Bad request
        [ProducesResponseType(403)] // Forbid
        public async Task<IActionResult> Ferme([FromQuery] KeyUidRno key, Dateur jusquA)
        {
            CarteUtilisateur carte = await _utilisateurService.CréeCarteUtilisateur(HttpContext.User);
            if (carte == null)
            {
                // fausse carte
                return Forbid();
            }

            if (!carte.EstPropriétaire(key.KeyParam))
            {
                return Forbid();
            }

            RetourDeService retour = await _service.Ferme(key, jusquA.Date);

            return SaveChangesActionResult(retour);
        }

        [HttpGet("/api/site/etat")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(403)] // Forbid
        [ProducesResponseType(404)] // Not found
        public async Task<IActionResult> Etat([FromQuery] KeyUidRno key)
        {
            CarteUtilisateur carte = await _utilisateurService.CréeCarteUtilisateur(HttpContext.User);
            if (carte == null)
            {
                // fausse carte
                return Forbid();
            }

            KeyParam param = key.KeyParam;
            Site site = await _service.Lit(param);

            if (!carte.EstPropriétaire(param) && !carte.EstClient(site.NomSite))
            {
                return Forbid();
            }

            return Ok(await _service.Etat(key));
        }

        protected override bool ListeEstPermis(CarteUtilisateur carte)
        {
            return true;
        }
        [HttpGet("/api/site/liste")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(403)] // Forbid
        [ProducesResponseType(404)] // Not found
        [AllowAnonymous]
        public new async Task<IActionResult> Liste()
        {
            return await base.Liste();
        }

        private Task<bool> EditeEstPermis(CarteUtilisateur carte, KeyParam param)
        {
            return Task.FromResult(carte.EstAdministrateur || carte.EstPropriétaire(param));
        }
        [HttpPut("/api/site/edite")]
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
            SiteVue vue = await _service.TrouveParNom(nomSite);
            if (vue == null)
            {
                return NotFound();
            }
            return Ok(vue);
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
