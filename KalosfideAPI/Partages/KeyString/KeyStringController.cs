using KalosfideAPI.Data.Keys;
using KalosfideAPI.Erreurs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyString
{
    public abstract class KeyStringController<T, TVue> : BaseController
        where T : AKeyString where TVue : AKeyString
    {
        protected IKeyStringService<T> __service;
        protected ITransformation<T, TVue> __transformation;

        public KeyStringController(
            IKeyStringService<T> service,
            ITransformation<T, TVue> transformation
            ) : base()
        {
            __service = service;
            __transformation = transformation;
            opérations = new List<Opération>
            {
                new Opération { Nom = nameof(Ajoute) },
                new Opération { Nom = nameof(Lit) },
                new Opération { Nom = nameof(Edite) },
                new Opération { Nom = nameof(Supprime) },
            };
        }

        protected AKeyString CréeAKey(string key)
        {
            try
            {
                var keyFabrique = new KeyFabrique(key);
                if (keyFabrique.KeyUId != null)
                {
                    return keyFabrique.KeyUId;
                }
                if (keyFabrique.KeyUIdRNo != null)
                {
                    return keyFabrique.KeyUIdRNo;
                }
                if (keyFabrique.KeyUIdRNoNo != null)
                {
                    return keyFabrique.KeyUIdRNoNo;
                }
            }
            catch (ArgumentException)
            {
            }
            return null;
        }

        public async Task<IActionResult> Ajoute(TVue vue)
        {
            T donnée = __transformation.CréeDonnée(vue);

            ErreurDeModel erreur = await __service.Validation(donnée);
            if (erreur != null)
            {
                erreur.AjouteAModelState(ModelState);
                return BadRequest(ModelState);
            }

            var revendications = Sécurité.RevendicationsFabrique.Revendications(HttpContext.User);
            bool permis = (PermiseAuPropriétaire(nameof(Ajoute))
                && revendications.PeutDevenirPropriétaire(donnée)) || revendications.EstAdministrateur;
            if (!permis)
            {
                return Forbid();
            }

            RetourDeService<T> retour = await __service.Ajoute(donnée);

            if (retour.Ok)
            {
                return CreatedAtAction(nameof(Lit), vue.Key, vue);
            }

            return SaveChangesActionResult(retour);
        }

        public async Task<IActionResult> Lit(string key)
        {
            AKeyString aKey = CréeAKey(key);
            if (aKey == null)
            {
                return BadRequest();
            }

            var revendications = Sécurité.RevendicationsFabrique.Revendications(HttpContext.User);
            bool permis = (PermiseAuPropriétaire(nameof(Lit)) && revendications.EstPropriétaire(aKey)) || revendications.EstAdministrateur;
            if (!permis)
            {
                return Forbid();
            }

            T donnée = await __service.Lit(aKey);
            if (donnée == null)
            {
                return NotFound();
            }

            TVue vue = __transformation.CréeVue(donnée);
            return Ok(vue);
        }

        public async Task<IActionResult> Liste(string key)
        {
            AKeyString aKey = CréeAKey(key);
            if (aKey == null)
            {
                return BadRequest();
            }

            var revendications = Sécurité.RevendicationsFabrique.Revendications(HttpContext.User);
            bool permis = (PermiseAuPropriétaire(nameof(Liste)) && revendications.EstPropriétaire(aKey)) || revendications.EstAdministrateur;
            if (!permis)
            {
                return Forbid();
            }

            List<T> donnée = await __service.Liste(aKey);
            if (donnée == null)
            {
                return NotFound();
            }

            IEnumerable<TVue> vue = __transformation.CréeVues(donnée);
            return Ok(vue);
        }

        public async Task<IActionResult> Liste(string key, OptionsDeListe options)
        {
            AKeyString aKey = CréeAKey(key);
            if (aKey == null)
            {
                return BadRequest();
            }

            var revendications = Sécurité.RevendicationsFabrique.Revendications(HttpContext.User);
            bool permis = (PermiseAuPropriétaire(nameof(Liste)) && revendications.EstPropriétaire(aKey)) || revendications.EstAdministrateur;
            if (!permis)
            {
                return Forbid();
            }

            List<T> donnée = await __service.ListeAOptions(aKey, options);
            if (donnée == null)
            {
                return NotFound();
            }

            IEnumerable<TVue> vue = __transformation.CréeVues(donnée);
            return Ok(vue);
        }

        private async Task<IActionResult> _Liste(string key, OptionsDeListe options)
        {
            var revendications = Sécurité.RevendicationsFabrique.Revendications(HttpContext.User);
            bool permis = revendications.EstAdministrateur;
            List<T> donnée = null;
            if (key == null)
            {
                if (!permis)
                {
                    return Forbid();
                }
                donnée = await __service.ListeAOptions(options);
            }
            else
            {
                AKeyString aKey = CréeAKey(key);
                if (aKey == null)
                {
                    return BadRequest();
                }

                permis = permis || (PermiseAuPropriétaire(nameof(Liste)) && revendications.EstPropriétaire(aKey));
                if (!permis)
                {
                    return Forbid();
                }

                donnée = await __service.ListeAOptions(aKey, options);
            }

            if (donnée == null)
            {
                return NotFound();
            }

            IEnumerable<TVue> vue = __transformation.CréeVues(donnée);
            return Ok(vue);
        }

        public async Task<IActionResult> Edite(TVue vue)
        {
            AKeyString aKey = vue;
            if (aKey == null)
            {
                return BadRequest();
            }

            var revendications = Sécurité.RevendicationsFabrique.Revendications(HttpContext.User);
            bool permis = (PermiseAuPropriétaire(nameof(Liste)) && revendications.EstPropriétaire(aKey)) || revendications.EstAdministrateur;
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

            donnée = await __service.Lit(aKey);
            if (donnée == null)
            {
                return NotFound();
            }

            __transformation.CopieVueDansDonnées(donnée, vue);

            RetourDeService<T> retour = await __service.Edite(donnée);

            return SaveChangesActionResult(retour);
        }

        public async Task<IActionResult> Supprime(string key)
        {
            AKeyString aKey = CréeAKey(key);
            if (aKey == null)
            {
                return BadRequest();
            }

            var revendications = Sécurité.RevendicationsFabrique.Revendications(HttpContext.User);
            bool permis = (PermiseAuPropriétaire(nameof(Liste)) && revendications.EstPropriétaire(aKey)) || revendications.EstAdministrateur;
            if (!permis)
            {
                return Forbid();
            }

            var donnée = await __service.Lit(aKey);
            if (donnée == null)
            {
                return NotFound();
            }

            var retour = await __service.Supprime(donnée);

            return SaveChangesActionResult(retour);
        }

    }
}
