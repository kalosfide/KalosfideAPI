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

namespace KalosfideAPI.Partages.KeyLong
{
    public abstract class KeyLongController<T, TVue> : BaseController where T : class, IKeyLong where TVue : class, IKeyLong
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

        protected bool EstAdministrateur()
        {
            var claims = HttpContext.User.Claims;
            if (claims == null)
            {
                return false;
            }
            return Sécurité.RevendicationsFabrique.EstAdministrateur(claims);
        }

        public async Task<IActionResult> Ajoute(TVue vue)
        {
            bool permis = EstAdministrateur();
            if (!permis)
            {
                return Forbid();
            }

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
                return CreatedAtAction(nameof(Lit), KeyLongFabrique.CréeKey(donnée), donnée);
            }

            return SaveChangesActionResult(retour);
        }

        public async Task<IActionResult> Lit(string param)
        {
            bool permis = EstAdministrateur();
            if (!permis)
            {
                return Forbid();
            }

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

            return Ok(donnée);
        }

        public async Task<IActionResult> Lit()
        {
            bool permis = EstAdministrateur();
            if (!permis)
            {
                return Forbid();
            }

            List<T> données = await __service.Lit();

            if (données == null)
            {
                return NotFound();
            }

            return Ok(données);
        }

        public async Task<IActionResult> Edite(TVue vue)
        {
            bool permis = EstAdministrateur();
            if (!permis)
            {
                return Forbid();
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

            __transformation.CopieVueDansDonnées(donnée, vue);

            RetourDeService<T> retour = await __service.Edite(donnée);

            return SaveChangesActionResult(retour);
        }

        public async Task<IActionResult> Supprime(string param)
        {
            bool permis = EstAdministrateur();
            if (!permis)
            {
                return Forbid();
            }

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

            var retour = await __service.Supprime(donnée);

            return SaveChangesActionResult(retour);
        }
    }
}