using KalosfideAPI.Data.Keys;
using KalosfideAPI.Erreurs;
using KalosfideAPI.Sécurité;
using KalosfideAPI.Utilisateurs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyParams
{
    public delegate Task<List<TVue>> ServiceVues<TVue>();

    /// <summary>
    /// permet de rendre des tables inaccessibles pendant que d'autres le sont
    /// </summary>
    /// <param name="carte"></param>
    /// <param name="donnée"></param>
    /// <returns></returns>
    public delegate Task<bool> DEcritVerrouillé<T>(CarteUtilisateur carte, T donnée) where T : AKeyBase;

    /// <summary>
    /// examine si l'user Http a le droit d'ajouter la donnée
    /// </summary>
    /// <param name="carte">CarteUtilisateur de l'user Http</param>
    /// <param name="param">KeyParam parent de la donnée</param>
    /// <returns>true si l'user Http a le droit d'ajouter la donnée</returns>
    public delegate Task<bool> DAjouteEstPermis(CarteUtilisateur carte, KeyParam param);

    /// <summary>
    /// examine si l'user Http a le droit de modifier la donnée
    /// </summary>
    /// <param name="carte">CarteUtilisateur de l'user Http</param>
    /// <param name="param">KeyParam parent de la donnée</param>
    /// <returns>true si l'user Http a le droit de modifier la donnée</returns>
    public delegate Task<bool> DEditeEstPermis(CarteUtilisateur carte, KeyParam param);

    /// <summary>
    /// examine si l'user Http a le droit de supprimer la donnée
    /// </summary>
    /// <param name="carte">CarteUtilisateur de l'user Http</param>
    /// <param name="param">KeyParam parent de la donnée</param>
    /// <returns>true si l'user Http a le droit de supprimer la donnée</returns>
    public delegate Task<bool> DSupprimeEstPermis(CarteUtilisateur carte, KeyParam param);

    public abstract class KeyParamController<T, TVue, TParam> : BaseController where T : AKeyBase where TVue : AKeyBase where TParam : KeyParam
    {
        protected IKeyParamService<T, TVue, TParam> __service;

        protected IUtilisateurService _utilisateurService;

        protected DEcritVerrouillé<T> dEcritVerrouillé;
        protected DAjouteEstPermis dAjouteEstPermis;
        protected DEditeEstPermis dEditeEstPermis;
        protected DSupprimeEstPermis dSupprimeEstPermis;

        public KeyParamController(IKeyParamService<T, TVue, TParam> service, IUtilisateurService utilisateurService)
        {
            __service = service;
            _utilisateurService = utilisateurService;
        }

        /// <summary>
        /// Fixe le numéro (Rno ou No suivant la surcharge) au premier disponible
        /// </summary>
        /// <param name="vue">vue à ajouter</param>
        /// <returns></returns>
        protected abstract Task FixeKeyParamAjout(TVue vue);

        public async Task<IActionResult> Ajoute(TVue vue)
        {
            CarteUtilisateur carte = await _utilisateurService.CréeCarteUtilisateur(HttpContext.User);
            if (carte == null)
            {
                // fausse carte
                return Forbid();
            }

            if (dAjouteEstPermis == null || !(await dAjouteEstPermis(carte, vue.KeyParamParent)))
            {
                return Forbid();
            }

            await FixeKeyParamAjout(vue);
            T donnée = __service.CréeDonnée(vue);

            if (dEcritVerrouillé != null && await dEcritVerrouillé(carte, donnée))
            {
                return Conflict();
            }

            DValideAjoute<T> dValideAjoute = __service.DValideAjoute();
            if (dValideAjoute != null)
            {
                await dValideAjoute(donnée, ModelState);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
            }

            RetourDeService<T> retour = await __service.Ajoute(donnée);

            if (retour.Ok)
            {
                return CreatedAtAction(nameof(Lit), vue.TexteKey, vue);
            }

            return SaveChangesActionResult(retour);
        }

        public async Task<IActionResult> Edite(TVue vue)
        {
            CarteUtilisateur carte = await _utilisateurService.CréeCarteUtilisateur(HttpContext.User);
            if (carte == null)
            {
                // fausse carte
                return Forbid();
            }

            if (dEditeEstPermis == null || !(await dEditeEstPermis(carte, vue.KeyParam)))
            {
                return Forbid();
            }

            T nouveau = __service.CréeDonnée(vue);

            if (dEcritVerrouillé != null && await dEcritVerrouillé(carte, nouveau))
            {
                return Conflict();
            }

            DValideEdite<T> dValideEdite = __service.DValideEdite();
            if (dValideEdite != null)
            {
                await dValideEdite(nouveau, ModelState);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
            }

            T donnée = await __service.Lit(vue.KeyParam as TParam);
            if (donnée == null)
            {
                return NotFound();
            }

            RetourDeService<T> retour = await __service.Edite(donnée, nouveau);

            return SaveChangesActionResult(retour);
        }
        public async Task<IActionResult> Supprime(TParam param)
        {
            CarteUtilisateur carte = await _utilisateurService.CréeCarteUtilisateur(HttpContext.User);
            if (carte == null)
            {
                // fausse carte
                return Forbid();
            }

            if (dSupprimeEstPermis == null || !(await dSupprimeEstPermis(carte, param)))
            {
                return Forbid();
            }

            var donnée = await __service.Lit(param);
            if (donnée == null)
            {
                return NotFound();
            }

            if (dEcritVerrouillé != null && await dEcritVerrouillé(carte, donnée))
            {
                return Conflict();
            }

            DValideSupprime<T> dValideSupprime = __service.DValideSupprime();
            if (dValideSupprime != null)
            {
                await dValideSupprime(donnée, ModelState);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
            }

            var retour = await __service.Supprime(donnée);

            return SaveChangesActionResult(retour);
        }

        protected virtual bool LitEstPermis(CarteUtilisateur carte, KeyParam param)
        {
            return false;
        }
        public async Task<IActionResult> Lit(TParam param)
        {
            CarteUtilisateur carte = await _utilisateurService.CréeCarteUtilisateur(HttpContext.User);
            if (carte == null)
            {
                // fausse carte
                return Forbid();
            }

            if (!LitEstPermis(carte, param))
            {
                return Forbid();
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
            CarteUtilisateur carte = await _utilisateurService.CréeCarteUtilisateur(HttpContext.User);
            if (carte == null)
            {
                // fausse carte
                return Forbid();
            }

            if (param != null && !ListeEstPermis(carte, param))
            {
                return Forbid();
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

    }
}