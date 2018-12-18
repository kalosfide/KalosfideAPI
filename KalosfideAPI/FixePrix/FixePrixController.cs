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

namespace KalosfideAPI.FixePrix
{
    [ApiController]
    [Route("UidRno")]
    [Authorize]
    public class FixePrixController : KeyUidRnoNoController<EtatPrix, FixePrix>
    {
        private IFixePrixService _service;

        public FixePrixController(IFixePrixService service) : base(service)
        {
            _service = service;
        }

        protected override Task FixeKeyParamAjout(FixePrix vue)
        {
            return Task.CompletedTask;
        }

        protected override Task<bool> AjouteEstPermis(CarteUtilisateur carte, KeyParam param)
        {
            return Task.FromResult(carte.EstPropriétaire(param));
        }
        [HttpPost("/api/fixeprix/ajoute")]
        [ProducesResponseType(201)] // created
        [ProducesResponseType(400)] // Bad request
        public new async Task<IActionResult> Ajoute(FixePrix vue)
        {
            return await base.Ajoute(vue);
        }

        protected override bool LitEstPermis(CarteUtilisateur carte, KeyParam param)
        {
            return carte.EstPropriétaire(param);
        }
        [HttpGet("/api/fixeprix/lit")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(404)] // Not found
        public async Task<IActionResult> Lit([FromQuery] KeyUidRnoNo param)
        {
            return await base.Lit(param.KeyParam);
        }
    }
}