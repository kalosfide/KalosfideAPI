using KalosfideAPI.Erreurs;
using KalosfideAPI.Sécurité;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages
{
    public abstract class KeyLongController<T, TVue> : SaveChangesController where T : class, IKeyLong where TVue : class, IKeyLong
    {
        protected readonly IAuthorizationService _autorisation;
        protected IKeyLongService<T> __service;
        protected ITransformation<T, TVue> __transformation;

        public KeyLongController(
            IAuthorizationService autorisation,
            IKeyLongService<T> service,
            ITransformation<T, TVue> transformation
            ) : base()
        {
            _autorisation = autorisation;
            __service = service;
            __transformation = transformation;
        }

        public async Task<IActionResult> Ajoute(TVue vue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            T donnée = __transformation.CréeDonnée(vue);

            ErreurDeModel erreur = await __service.Validation(donnée);
            if (erreur != null)
            {
                erreur.AjouteAModelState(ModelState);
                return BadRequest(ModelState);
            }

            OperationAuthorizationRequirement[] requirements = new OperationAuthorizationRequirement[]
                { BaseActions.Ajoute.Requirement };
            var permis = await _autorisation.AuthorizeAsync(HttpContext.User, donnée, requirements);
            if (!permis.Succeeded)
            {
                return Forbid();
            }

            BaseServiceRetour<T> retour = await __service.Ajoute(donnée);

            if (retour.Ok)
            {
                return CreatedAtAction(nameof(Lit), KeyLongFabrique.CréeKey(donnée), donnée);
            }

            return SaveChangesActionResult(retour);
        }

        public async Task<IActionResult> Lit(string param)
        {
            var key = KeyLongFabrique.CréeKey(param);
            if (key == null)
            {
                return BadRequest();
            }

            var donnée = await __service.Lit(key);
            if (donnée == null)
            {
                return NotFound();
            }

            OperationAuthorizationRequirement[] requirements = new OperationAuthorizationRequirement[]
            { BaseActions.Lit.Requirement };
            var permis = await _autorisation.AuthorizeAsync(HttpContext.User, donnée, requirements);
            if (!permis.Succeeded)
            {
                return Forbid();
            }

            return Ok(donnée);
        }

        public async Task<IActionResult> Lit()
        {
            List<T> données = await __service.Lit();

            if (données == null)
            {
                return NotFound();
            }

            return Ok(données);
        }

        public async Task<IActionResult> Edite(TVue vue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            T donnée = __transformation.CréeDonnée(vue);

            ErreurDeModel erreur = await __service.Validation(donnée);
            if (erreur != null)
            {
                erreur.AjouteAModelState(ModelState);
                return BadRequest(ModelState);
            }

            donnée = await __service.Lit(KeyLongFabrique.CréeKey(vue));

            if (donnée == null)
            {
                return NotFound();
            }

            OperationAuthorizationRequirement[] requirements = new OperationAuthorizationRequirement[]
            { BaseActions.Edite.Requirement };
            var permis = await _autorisation.AuthorizeAsync(HttpContext.User, donnée, requirements);
            if (!permis.Succeeded)
            {
                return Forbid();
            }

            __transformation.CopieVueDansDonnées(donnée, vue);

            BaseServiceRetour<T> retour = await __service.Edite(donnée);

            return SaveChangesActionResult(retour);
        }

        public async Task<IActionResult> Supprime(string param)
        {
            var key = KeyLongFabrique.CréeKey(param);
            if (key == null)
            {
                return BadRequest();
            }

            var donnée = await __service.Lit(key);
            if (donnée == null)
            {
                return NotFound();
            }

            OperationAuthorizationRequirement[] requirements = new OperationAuthorizationRequirement[]
            { BaseActions.Supprime.Requirement };
            var permis = await _autorisation.AuthorizeAsync(HttpContext.User, donnée, requirements);
            if (!permis.Succeeded)
            {
                return Forbid();
            }

            var retour = await __service.Supprime(donnée);

            return SaveChangesActionResult(retour);
        }
    }
}