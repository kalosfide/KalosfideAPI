using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KalosfideAPI.Data;
using KalosfideAPI.Partages.KeyString;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KalosfideAPI.Produits
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduitController : KeyRIdNoController<Produit, ProduitVue>
    {
        public ProduitController(IProduitService service, IProduitTransformation transformation) : base(service, transformation)
        {
        }

        private new IProduitService _service { get => __service as IProduitService; }
        private new IProduitTransformation _transformation { get => __transformation as IProduitTransformation; }

        // POST api/utilisateur/ajoute
        [HttpPost]
        [ProducesResponseType(201)] // created
        [ProducesResponseType(400)] // Bad request
        public new async Task<IActionResult> Ajoute(ProduitVue vue)
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

        public new async Task<IActionResult> Edite(ProduitVue vue)
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
    }
}