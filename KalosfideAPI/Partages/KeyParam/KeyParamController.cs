using KalosfideAPI.Data.Keys;
using KalosfideAPI.Erreurs;
using KalosfideAPI.Sécurité;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyParams
{
    public abstract class KeyParamController<T, TVue, TParam> : BaseController where T : AKeyBase where TVue : AKeyBase where TParam : KeyParam
    {
        protected IKeyParamService<T, TParam> __service;
        protected IKeyParamTransformation<T, TVue> __transformation;

        public KeyParamController(
            IKeyParamService<T, TParam> service,
            IKeyParamTransformation<T, TVue> transformation
            )
        {
            __service = service;
            __transformation = transformation;
        }

        protected virtual bool AjouteEstPermis(CarteUtilisateur carte, KeyParam param)
        {
            return false;
        }

        protected abstract Task FixeKeyParamAjout(TVue vue);
        public async Task<IActionResult> Ajoute(TVue vue)
        {
            CarteUtilisateur carte = new CarteUtilisateur();
            carte.PrendClaims(HttpContext.User);

            if (!AjouteEstPermis(carte, vue.KeyParamParent))
            {
                return StatusCode(403);
            }

            await FixeKeyParamAjout(vue);
            T donnée = __transformation.CréeDonnée(vue);

            ErreurDeModel erreur = await __service.Validation(donnée);
            if (erreur != null)
            {
                erreur.AjouteAModelState(ModelState);
                return BadRequest(ModelState);
            }

            RetourDeService<T> retour = await __service.Ajoute(donnée);

            if (retour.Ok)
            {
                return CreatedAtAction(nameof(Lit), vue.TexteKey, vue);
            }

            return SaveChangesActionResult(retour);
        }

        protected virtual bool LitEstPermis(CarteUtilisateur carte, KeyParam param)
        {
            return false;
        }
        public async Task<IActionResult> Lit(TParam param)
        {

            CarteUtilisateur carte = new CarteUtilisateur();
            carte.PrendClaims(HttpContext.User);

            if (!LitEstPermis(carte, param))
            {
                return StatusCode(403);
            }

            T donnée = await __service.Lit(param);
            if (donnée == null)
            {
                return NotFound();
            }

            TVue vue = __transformation.CréeVue(donnée);
            return Ok(vue);
        }

        protected virtual bool ListeEstPermis(CarteUtilisateur carte)
        {
            return false;
        }
        protected virtual bool ListeEstPermis(CarteUtilisateur carte, KeyParam param)
        {
            return ListeEstPermis(carte);
        }
        public async Task<IActionResult> Liste(KeyParam param)
        {
            CarteUtilisateur carte = new CarteUtilisateur();
            carte.PrendClaims(HttpContext.User);

            if (!ListeEstPermis(carte, param))
            {
                return StatusCode(403);
            }

            List<T> donnée = await __service.Liste(param);
            if (donnée == null)
            {
                return NotFound();
            }

            IEnumerable<TVue> vue = __transformation.CréeVues(donnée);
            return Ok(vue);
        }
        public async Task<IActionResult> Liste()
        {
            CarteUtilisateur carte = new CarteUtilisateur();
            carte.PrendClaims(HttpContext.User);

            if (!ListeEstPermis(carte))
            {
                return StatusCode(403);
            }

            List<T> donnée = await __service.Liste();
            if (donnée == null)
            {
                return NotFound();
            }

            IEnumerable<TVue> vue = __transformation.CréeVues(donnée);
            return Ok(vue);
        }

        protected virtual bool EditeEstPermis(CarteUtilisateur carte, KeyParam param)
        {
            return false;
        }
        public async Task<IActionResult> Edite(TVue vue)
        {
            CarteUtilisateur carte = new CarteUtilisateur();
            carte.PrendClaims(HttpContext.User);
            if (!EditeEstPermis(carte, vue.KeyParam))
            {
                return StatusCode(403);
            }

            T donnée = __transformation.CréeDonnée(vue);

            ErreurDeModel erreur = await __service.Validation(donnée);
            if (erreur != null)
            {
                erreur.AjouteAModelState(ModelState);
                return BadRequest(ModelState);
            }

            donnée = await __service.Lit(vue.KeyParam as TParam);
            if (donnée == null)
            {
                return NotFound();
            }

            __transformation.CopieVueDansDonnées(donnée, vue);

            RetourDeService<T> retour = await __service.Edite(donnée);

            return SaveChangesActionResult(retour);
        }

        protected virtual bool SupprimeEstPermis(CarteUtilisateur carte, KeyParam param)
        {
            return false;
        }
        public async Task<IActionResult> Supprime(TParam param)
        {

            CarteUtilisateur carte = new CarteUtilisateur();
            carte.PrendClaims(HttpContext.User);

            if (!SupprimeEstPermis(carte, param))
            {
                return StatusCode(403);
            }

            var donnée = await __service.Lit(param);
            if (donnée == null)
            {
                return NotFound();
            }

            var retour = await __service.Supprime(donnée);

            return SaveChangesActionResult(retour);
        }

    }
}