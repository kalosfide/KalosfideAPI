using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Partages;
using KalosfideAPI.Partages.KeyParams;
using KalosfideAPI.Sécurité;
using KalosfideAPI.Sites;
using KalosfideAPI.Utilisateurs;
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

        public CommandeController(ICommandeService service, IUtilisateurService utilisateurService) : base(service, utilisateurService)
        {
            dEcritVerrouillé = EcritVerrouillé;
        }

        private ICommandeService _service { get => __service as ICommandeService; }

        private Task<bool> EcritVerrouillé(CarteUtilisateur carte, Commande donnée)
        {
            KeyUidRno keyClient = new KeyUidRno { Uid = donnée.Uid, Rno = donnée.Rno };
            SiteVue site = carte.SiteClient(keyClient.KeyParam);
            return Task.FromResult(site == null || !site.Ouvert);
        }

        /// <summary>
        /// retourne la vue de la dernière Commande d'un client
        /// </summary>
        /// <param name="keyClient">key du client</param>
        /// <returns></returns>
        [HttpGet("/api/commande/enCours")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(403)] // Forbid
        [ProducesResponseType(404)] // Not found
        public async Task<IActionResult> EnCours([FromQuery] KeyUidRno keyClient)
        {
            CarteUtilisateur carte = await _utilisateurService.CréeCarteUtilisateur(HttpContext.User);
            if (carte == null)
            {
                // fausse carte
                return Forbid();
            }

            if (!carte.EstPropriétaire(keyClient.KeyParam))
            {
                return Forbid();
            }

            CommandeVue vue = await _service.CommandeEnCours(keyClient);
            if (vue == null)
            {
                return NotFound();
            }

            return Ok(vue);
        }

        [HttpPost("/api/commande/envoiBon")]
        [ProducesResponseType(201)] // created
        [ProducesResponseType(400)] // Bad request
        [ProducesResponseType(403)] // Forbid
        [ProducesResponseType(409)] // Conflict
        public async Task<IActionResult> EnvoiBon(CommandeVue vue)
        {
            CarteUtilisateur carte = await _utilisateurService.CréeCarteUtilisateur(HttpContext.User);
            if (carte == null)
            {
                // fausse carte
                return Forbid();
            }

            if (!carte.EstPropriétaire(vue.KeyParamParent))
            {
                return Forbid();
            }

            KeyUidRno keySite = new KeyUidRno { Uid = vue.Uid, Rno = vue.Rno };
            SiteVue site = carte.SiteFournisseur(keySite.KeyParam);
            if (site == null || !site.Ouvert)
            {
                return Conflict();
            }

            RetourDeService retour = await _service.EnvoiBon(vue);

            if (!retour.Ok)
            {
                return BadRequest();
            }

            return SaveChangesActionResult(retour);
        }

        /// <summary>
        /// retourne le nombre des Commandes ouvertes d'un fournisseur
        /// </summary>
        /// <param name="keyFournisseur">key du fournisseur</param>
        /// <returns></returns>
        [HttpGet("/api/commande/nbouvertes")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(403)] // Forbid
        [ProducesResponseType(404)] // Not found
        public async Task<IActionResult> NbOuvertes([FromQuery] KeyUidRno keyFournisseur)
        {
            CarteUtilisateur carte = await _utilisateurService.CréeCarteUtilisateur(HttpContext.User);
            if (carte == null)
            {
                // fausse carte
                return Forbid();
            }

            if (!carte.EstPropriétaire(keyFournisseur.KeyParam))
            {
                return Forbid();
            }

            int nb = await _service.NbOuvertes(keyFournisseur);

            return Ok(nb);
        }

        /// <summary>
        /// retourne les vues à accepter des Commandes ouvertes d'un fournisseur
        /// </summary>
        /// <param name="keyFournisseur">key du fournisseur</param>
        /// <returns></returns>
        [HttpGet("/api/commande/ouvertes")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(403)] // Forbid
        [ProducesResponseType(404)] // Not found
        public async Task<IActionResult> Ouvertes([FromQuery] KeyUidRno keyFournisseur)
        {
            CarteUtilisateur carte = await _utilisateurService.CréeCarteUtilisateur(HttpContext.User);
            if (carte == null)
            {
                // fausse carte
                return Forbid();
            }

            if (!carte.EstPropriétaire(keyFournisseur.KeyParam))
            {
                return Forbid();
            }

            List<CommandeVue> vues = await _service.VuesOuvertes(keyFournisseur);

            return Ok(vues);
        }

        [HttpPost("/api/commande/enregistre")]
        [ProducesResponseType(201)] // created
        [ProducesResponseType(400)] // Bad request
        [ProducesResponseType(403)] // Forbid
        [ProducesResponseType(409)] // Conflict
        public async Task<IActionResult> Enregistre([FromQuery] KeyUidRno keyFournisseur, List<CommandeVue> vues)
        {
            CarteUtilisateur carte = await _utilisateurService.CréeCarteUtilisateur(HttpContext.User);
            if (carte == null)
            {
                // fausse carte
                return Forbid();
            }

            if (!carte.EstPropriétaire(keyFournisseur.KeyParam))
            {
                return Forbid();
            }

            SiteVue site = carte.SiteFournisseur(keyFournisseur.KeyParam);
            if (site == null || site.Ouvert)
            {
                return Conflict();
            }

            List<Commande> vérifiées = await _service.VérifieEnregistre(keyFournisseur, vues);

            if (vérifiées == null)
            {
                return BadRequest();
            }

            RetourDeService retour = await _service.Enregistre(keyFournisseur, vérifiées);

            return SaveChangesActionResult(retour);
        }

        /* INUTILISE */
        /// <summary>
        /// ferme (fixe la date) des Commandes ouvertes (sans date) d'un fournisseur
        /// </summary>
        /// <param name="keyFournisseur">key du fournisseur</param>
        /// <returns></returns>
        [HttpPut("/api/commande/fermeOuvertes")]
        [ProducesResponseType(200)] // ok
        [ProducesResponseType(403)] // Forbid
        [ProducesResponseType(400)] // Bad request
        public async Task<IActionResult> FermeOuvertes([FromQuery] KeyUidRno keyFournisseur)
        {
            CarteUtilisateur carte = await _utilisateurService.CréeCarteUtilisateur(HttpContext.User);
            if (carte == null)
            {
                // fausse carte
                return Forbid();
            }

            if (!carte.EstPropriétaire(keyFournisseur.KeyParam))
            {
                return Forbid();
            }

            RetourDeService retour = await _service.FermeOuvertes(keyFournisseur);

            if (retour.Ok)
            {
                return Ok();
            }

            return SaveChangesActionResult(retour);
        }

        [HttpGet("/api/commande/nouvelles")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(403)] // Forbid
        [ProducesResponseType(404)] // Not found
        public async Task<IActionResult> Nouvelles([FromQuery] KeyUidRno key)
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
            return Ok(await _service.NouvellesCommandes(key));
        }

        [HttpGet("/api/commande/dernierNo")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(403)] // Forbid
        [ProducesResponseType(404)] // Not found
        public async Task<IActionResult> DernierNo([FromQuery] KeyParam param)
        {
            return Ok(await _service.DernierNo(param));
        }

        protected override bool LitEstPermis(CarteUtilisateur carte, KeyParam param)
        {
            return carte.EstPropriétaire(param);
        }
        [HttpGet("/api/commande/lit")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(403)] // Forbid
        [ProducesResponseType(404)] // Not found
        public async Task<IActionResult> Lit([FromQuery] KeyUidRnoNo param)
        {
            return await base.Lit(param.KeyParam);
        }
        protected override bool ListeEstPermis(CarteUtilisateur carte, KeyParam param)
        {
            return carte.EstPropriétaire(param);
        }
        [HttpGet("/api/commande/liste")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(403)] // Forbid
        [ProducesResponseType(404)] // Not found
        public new async Task<IActionResult> Liste([FromQuery] KeyParam param)
        {
            return await base.Liste(param);
        }
    }
}
