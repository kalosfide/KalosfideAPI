using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Partages;
using KalosfideAPI.Partages.KeyParams;
using KalosfideAPI.Sécurité;
using KalosfideAPI.Utilisateurs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KalosfideAPI.Livraisons
{
    [ApiController]
    [Route("UidRnoNo")]
    [Authorize]
    public class LivraisonController : KeyUidRnoNoController<Livraison, LivraisonVue>
    {
        public LivraisonController(ILivraisonService service, IUtilisateurService utilisateurService) : base(service, utilisateurService)
        {
        }

        private ILivraisonService _service { get => __service as ILivraisonService; }

        /// <summary>
        /// retourne la vue de la dernière Livraison d'un Fournisseur
        /// </summary>
        /// <param name="keyFournisseur">key du client</param>
        /// <returns></returns>
        [HttpGet("/api/livraison/enCours")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(403)] // Forbid
        [ProducesResponseType(404)] // Not found
        public async Task<IActionResult> EnCours([FromQuery] KeyUidRno keyFournisseur)
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
            return Ok(await _service.LivraisonEnCours(keyFournisseur));
        }


        [HttpGet("/api/livraison/dernierNo")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(403)] // Forbid
        [ProducesResponseType(404)] // Not found
        public async Task<IActionResult> DernierNo([FromQuery] KeyParam param)
        {
            CarteUtilisateur carte = await _utilisateurService.CréeCarteUtilisateur(HttpContext.User);
            if (carte == null)
            {
                // fausse carte
                return Forbid();
            }

            if (!carte.EstPropriétaire(param))
            {
                return Forbid();
            }
            return Ok(await _service.DernierNo(param));
        }

        protected override bool LitEstPermis(CarteUtilisateur carte, KeyParam param)
        {
            return carte.EstPropriétaire(param);
        }
        [HttpGet("/api/livraison/lit")]
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
        [HttpGet("/api/livraison/liste")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(403)] // Forbid
        [ProducesResponseType(404)] // Not found
        public new async Task<IActionResult> Liste([FromQuery] KeyParam param)
        {
            return await base.Liste(param);
        }
    }
}
