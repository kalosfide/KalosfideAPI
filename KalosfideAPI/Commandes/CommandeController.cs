using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Partages;
using KalosfideAPI.Partages.KeyParams;
using KalosfideAPI.Sécurité;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Commandes
{
    [ApiController]
    [Route("UidRnoNo")]
    [Authorize]
    public class CommandeController : KeyUidRnoNoController<Commande, CommandeVue>
    {
        public CommandeController(ICommandeService service) : base(service)
        {
        }

        private ICommandeService _service { get => __service as ICommandeService; }


        [HttpPost("/api/commande/ajoute")]
        [ProducesResponseType(201)] // created
        [ProducesResponseType(400)] // Bad request
        public new async Task<IActionResult> Ajoute(CommandeVue vue)
        {
            CarteUtilisateur carte = new CarteUtilisateur();
            carte.PrendClaims(HttpContext.User);

            if (!carte.EstPropriétaire(vue.KeyParamParent))
            {
                return StatusCode(403);
            }

            RetourDeService<Commande> retour = await _service.AjouteVue(vue);

            if (retour.Ok)
            {
                return CreatedAtAction(nameof(Lit), vue.TexteKey, vue);
            }

            return SaveChangesActionResult(retour);
        }

        protected override bool LitEstPermis(CarteUtilisateur carte, KeyParam param)
        {
            return carte.EstPropriétaire(param);
        }
        [HttpGet("/api/commande/lit")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(404)] // Not found
        public async Task<IActionResult> Lit([FromQuery] KeyUidRnoNo param)
        {
            if (param.No == -1)
            {
                param.No = await _service.DernierNo(param.Uid, param.Rno);
            }
            return await base.Lit(param.KeyParam);
        }
        [HttpGet("/api/commande/ouverte")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(404)] // Not found
        /**
         * retourne la Commande sans date, en crée une s'il le faut
         */
        public async Task<IActionResult> Ouverte([FromQuery] KeyParam param)
        {
            CarteUtilisateur carte = new CarteUtilisateur();
            carte.PrendClaims(HttpContext.User);

            if (!carte.EstPropriétaire(param))
            {
                return StatusCode(403);
            }

            return Ok(await _service.CommandeOuverte(new KeyUidRno(param)));
        }

        protected override bool ListeEstPermis(CarteUtilisateur carte, KeyParam param)
        {
            return carte.EstPropriétaire(param);
        }
        [HttpGet("/api/commande/liste")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(404)] // Not found
        public new async Task<IActionResult> Liste([FromQuery] KeyParam param)
        {
            return await base.Liste(param);
        }

        protected override async Task<bool> EditeEstPermis(CarteUtilisateur carte, KeyParam param)
        {
            return carte.EstPropriétaire(param) && await _service.CommandeOuverte(new KeyUidRno(param)) != null;
        }
        [HttpPut("/api/commande/edite")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(404)] // Not found
        public new async Task<IActionResult> Edite(CommandeVue vue)
        {
            return await base.Edite(vue);
        }
    }
}
