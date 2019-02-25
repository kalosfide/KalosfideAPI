using System.Threading.Tasks;
using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Partages.KeyParams;
using KalosfideAPI.Sécurité;
using KalosfideAPI.Sites;
using KalosfideAPI.Utilisateurs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KalosfideAPI.Produits
{
    [ApiController]
    [Route("UidRnoNo")]
    [Authorize]
    public class ProduitController : KeyUidRnoNoController<Produit, ProduitVue>
    {
        public ProduitController(IProduitService service, IUtilisateurService utilisateurService) : base(service, utilisateurService)
        {
            dEcritVerrouillé = EcritVerrouillé;
            dAjouteEstPermis = AjouteEstPermis;
            dEditeEstPermis = EditeEstPermis;
            dSupprimeEstPermis = SupprimeEstPermis;
        }

        private IProduitService _service { get => __service as IProduitService; }

        private Task<bool> EcritVerrouillé(CarteUtilisateur carte, Produit donnée)
        {
            KeyUidRno keySite = new KeyUidRno { Uid = donnée.Uid, Rno = donnée.Rno };
            SiteVue site = carte.SiteFournisseur(keySite.KeyParam);
            return Task.FromResult(site == null || site.Ouvert);
        }

        private Task<bool> AjouteEstPermis(CarteUtilisateur carte, KeyParam param)
        {
            return Task.FromResult(carte.EstPropriétaire(param));
        }

        private Task<bool> EditeEstPermis(CarteUtilisateur carte, KeyParam param)
        {
            return Task.FromResult(carte.EstPropriétaire(param));
        }

        private Task<bool> SupprimeEstPermis(CarteUtilisateur carte, KeyParam param)
        {
            return Task.FromResult(carte.EstPropriétaire(param));
        }

        [HttpPost("/api/produit/ajoute")]
        [ProducesResponseType(201)] // created
        [ProducesResponseType(400)] // Bad request
        [ProducesResponseType(403)] // Forbid
        [ProducesResponseType(409)] // Conflict
        public new async Task<IActionResult> Ajoute(ProduitVue vue)
        {
            return await base.Ajoute(vue);
        }

        protected override bool LitEstPermis(CarteUtilisateur carte, KeyParam param)
        {
            return true;
        }
        [HttpGet("/api/produit/lit")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(403)] // Forbid
        [ProducesResponseType(404)] // Not found
        public async Task<IActionResult> Lit([FromQuery] KeyUidRnoNo param)
        {
            return await base.Lit(param.KeyParam);
        }

        protected override bool ListeEstPermis(CarteUtilisateur carte, KeyParam param)
        {
            return true;
        }
        [HttpGet("/api/produit/liste")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(403)] // Forbid
        [ProducesResponseType(404)] // Not found
        [AllowAnonymous]
        public new async Task<IActionResult> Liste([FromQuery] KeyParam param)
        {
            return await base.Liste(param);
        }
        [HttpGet("/api/produit/disponibles")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(403)] // Forbid
        [ProducesResponseType(404)] // Not found
        [AllowAnonymous]
        public async Task<IActionResult> Disponibles([FromQuery] KeyParam param)
        {
            return await Liste(param, (ProduitVue vue) => vue.Prix > 0);
        }
        [HttpPut("/api/produit/edite")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(403)] // Forbid
        [ProducesResponseType(404)] // Not found
        [ProducesResponseType(409)] // Conflict
        public new async Task<IActionResult> Edite(ProduitVue vue)
        {
            return await base.Edite(vue);
        }

        [HttpGet("/api/produit/nomPris/{nom}")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(403)] // Forbid
        [ProducesResponseType(404)] // Not found
        public async Task<IActionResult> NomPris(string nom)
        {
            return Ok(await _service.NomPris(nom));
        }

        [HttpGet("/api/produit/nomPrisParAutre/{nom}")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(403)] // Forbid
        [ProducesResponseType(404)] // Not found
        public async Task<IActionResult> NomPrisParAutre([FromQuery] KeyUidRnoNo key, string nom)
        {
            return Ok(await _service.NomPrisParAutre(key, nom));
        }
    }
}