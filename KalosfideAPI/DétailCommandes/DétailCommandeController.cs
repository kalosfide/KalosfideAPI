using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Partages.KeyParams;
using KalosfideAPI.Sécurité;
using KalosfideAPI.Utilisateurs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// INUTILISE

namespace KalosfideAPI.DétailCommandes
{
    [ApiController]
    [Route("UidRnoNo")]
    [Authorize]
    public class DétailCommandeController : KeyParamController<DétailCommande, DétailCommandeVue, KeyParam>
    {
        public DétailCommandeController(IDétailCommandeService service, IUtilisateurService utilisateurService) : base(service, utilisateurService)
        {
            dAjouteEstPermis = AjouteEstPermis;
            dEditeEstPermis = EditeEstPermis;
        }

        private IDétailCommandeService _service { get => __service as IDétailCommandeService; }

        private Task<bool> AjouteEstPermis(CarteUtilisateur carte, KeyParam param)
        {
            return Task.FromResult(carte.EstPropriétaire(param));
        }

        private Task<bool> EditeEstPermis(CarteUtilisateur carte, KeyParam param)
        {
            return Task.FromResult(carte.EstPropriétaire(param));
        }

        protected override Task FixeKeyParamAjout(DétailCommandeVue vue)
        {
            return Task.CompletedTask;
        }
        [HttpPost("/api/detailcommande/ajoute")]
        [ProducesResponseType(201)] // created
        [ProducesResponseType(400)] // Bad request
        public new async Task<IActionResult> Ajoute(DétailCommandeVue vue)
        {
            return await base.Ajoute(vue);
        }

        protected override bool LitEstPermis(CarteUtilisateur carte, KeyParam param)
        {
            return carte.EstPropriétaire(param);
        }
        [HttpGet("/api/detailcommande/lit")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(404)] // Not found
        public async Task<IActionResult> Lit([FromQuery] KeyUidRnoNoD param)
        {
            return await base.Lit(param.KeyParam);
        }

        protected override bool ListeEstPermis(CarteUtilisateur carte, KeyParam param)
        {
            return carte.EstPropriétaire(param);
        }
        [HttpGet("/api/detailcommande/liste")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(404)] // Not found
        public new async Task<IActionResult> Liste([FromQuery] KeyParam param)
        {
            return await base.Liste(param);
        }
        [HttpPut("/api/detailcommande/edite")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(404)] // Not found
        public new async Task<IActionResult> Edite(DétailCommandeVue vue)
        {
            return await base.Edite(vue);
        }
    }
}