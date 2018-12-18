using KalosfideAPI.Data.Keys;
using KalosfideAPI.Erreurs;
using KalosfideAPI.Sécurité;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyParams
{
    public delegate Task<List<TVue>> ServiceVues<TVue>();

    public abstract class KeyParamController<T, TVue, TParam> : BaseController where T : AKeyBase where TVue : AKeyBase where TParam : KeyParam
    {
        protected IKeyParamService<T, TVue, TParam> __service;

        public KeyParamController(
            IKeyParamService<T, TVue, TParam> service
            )
        {
            __service = service;
        }

        protected virtual Task<bool> AjouteEstPermis(CarteUtilisateur carte, KeyParam param)
        {
            return Task.FromResult(false);
        }
        protected virtual Task ValideAjoute(T donnée, ModelStateDictionary modelState)
        {
            return Task.CompletedTask;
        }
        protected abstract Task FixeKeyParamAjout(TVue vue);
        public async Task<IActionResult> Ajoute(TVue vue)
        {
            CarteUtilisateur carte = new CarteUtilisateur();
            carte.PrendClaims(HttpContext.User);

            if (!(await AjouteEstPermis(carte, vue.KeyParamParent)))
            {
                return StatusCode(403);
            }

            await FixeKeyParamAjout(vue);
            T donnée = __service.CréeDonnée(vue);

            await ValideAjoute(donnée, ModelState);
            if (!ModelState.IsValid)
            {
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

            TVue vue = await __service.LitVue(param);
            if (vue == null)
            {
                return NotFound();
            }

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
        protected async Task<IActionResult> Liste(ServiceVues<TVue> serviceVues, KeyParam param)
        {
            CarteUtilisateur carte = new CarteUtilisateur();
            carte.PrendClaims(HttpContext.User);

            if (param != null && !ListeEstPermis(carte, param))
            {
                return StatusCode(403);
            }

            List<TVue> vues = await serviceVues();
            if (vues == null)
            {
                return NotFound();
            }

            return Ok(vues);
        }
        public async Task<IActionResult> Liste(KeyParam param)
        {
            return await Liste(() => __service.Liste(param), param);
        }
        public async Task<IActionResult> Liste()
        {
            return await Liste(() => __service.Liste(), null);
        }
        protected async Task<IActionResult> Liste(KeyParam param, ValideFiltre<TVue> valide)
        {
            return await Liste(() => __service.Liste(param, valide), param);
        }
        protected async Task<IActionResult> Liste(ValideFiltre<TVue> valide)
        {
            return await Liste(() => __service.Liste(valide), null);
        }

        protected virtual Task<bool> EditeEstPermis(CarteUtilisateur carte, KeyParam param)
        {
            return Task.FromResult(false);
        }
        protected virtual Task ValideEdite(T donnée, ModelStateDictionary modelState)
        {
            return Task.CompletedTask;
        }
        public async Task<IActionResult> Edite(TVue vue)
        {
            CarteUtilisateur carte = new CarteUtilisateur();
            carte.PrendClaims(HttpContext.User);
            if (!(await EditeEstPermis(carte, vue.KeyParam)))
            {
                return StatusCode(403);
            }

            T donnée = __service.CréeDonnée(vue);

            await ValideEdite(donnée, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            donnée = await __service.Lit(vue.KeyParam as TParam);
            if (donnée == null)
            {
                return NotFound();
            }

            await __service.CopieVueDansDonnées(donnée, vue);

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