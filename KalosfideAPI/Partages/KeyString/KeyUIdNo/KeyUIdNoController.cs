using KalosfideAPI.Data.Keys;
using KalosfideAPI.Erreurs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyString
{
    public abstract class KeyUIdRNoController<T, TVue> : KeyStringController<T, TVue>
        where T: AKeyUIdRNo where TVue: AKeyUIdRNoVue
    {
        protected IKeyUIdRNoService<T> _service { get { return __service as IKeyUIdRNoService<T>; } }
        protected ITransformation<T, TVue> _transformation { get { return __transformation; } }

        public KeyUIdRNoController(
            IKeyUIdRNoService<T> service,
            ITransformation<T,TVue> transformation
            ) : base(service, transformation)
        {
            opérations.Add(new Opération { Nom = nameof(DernierNo) });
        }

        public async Task<IActionResult> DernierNo(string key)
        {
            KeyUId aKey = null;
            try
            {
                var keyFabrique = new KeyFabrique(key);
                if (keyFabrique.KeyUId != null)
                {
                    aKey = keyFabrique.KeyUId;
                }
            }
            catch (ArgumentException)
            {
            }
            if (aKey == null)
            {
                return BadRequest();
            }

            var revendications = Sécurité.RevendicationsFabrique.Revendications(HttpContext.User);
            bool permis = (PermiseAuPropriétaire(nameof(Ajoute)) && revendications.EstPropriétaire(aKey)) || revendications.EstAdministrateur;
            if (!permis)
            {
                return Forbid();
            }

            return Ok(await _service.DernierNo(aKey));
        }

    }
}