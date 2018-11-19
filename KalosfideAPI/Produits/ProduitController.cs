using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Partages.KeyParams;
using KalosfideAPI.Sécurité;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KalosfideAPI.Produits
{
    [ApiController]
    [Route("UidRno")]
    [Authorize]
    public class ProduitController : KeyUidRnoNoController<Produit, ProduitVue>
    {
        public ProduitController(IProduitService service, IProduitTransformation transformation) : base(service, transformation)
        {
        }

        private IProduitService _service { get => __service as IProduitService; }
        private IProduitTransformation _transformation { get => __transformation as IProduitTransformation; }

        protected override bool AjouteEstPermis(CarteUtilisateur carte, KeyParam param)
        {
            return carte.EstPropriétaire(param);
        }
        [HttpPost("/api/produit/ajoute")]
        [ProducesResponseType(201)] // created
        [ProducesResponseType(400)] // Bad request
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
        [ProducesResponseType(404)] // Not found
        [AllowAnonymous]
        public new async Task<IActionResult> Liste([FromQuery] KeyParam param)
        {
            return await base.Liste(param);
        }

        protected override bool EditeEstPermis(CarteUtilisateur carte, KeyParam param)
        {
            return carte.EstPropriétaire(param);
        }
        [HttpPut("/api/produit/edite")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(404)] // Not found
        public new async Task<IActionResult> Edite(ProduitVue vue)
        {
            return await base.Edite(vue);
        }
    }
}