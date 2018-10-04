using KalosfideAPI.Data.Keys;
using KalosfideAPI.Erreurs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyString
{
    public abstract class KeyUIdRNoController<T, TVue> : KeyStringController<T, TVue>
        where T: AKeyUIdRNo where TVue: AKeyUIdRNo
    {
        protected IKeyUIdRNoService<T> _service { get { return __service as IKeyUIdRNoService<T>; } }
        protected IKeyUIdRNoTransformation<T, TVue> _transformation { get { return __transformation as IKeyUIdRNoTransformation<T, TVue>; } }

        public KeyUIdRNoController(
            IKeyUIdRNoService<T> service,
            IKeyUIdRNoTransformation<T,TVue> transformation
            ) : base(service, transformation)
        {
            opérations.Add(new Opération { Nom = nameof(DernierNo) });
        }

        protected override AKeyBase CréeAKey(string texteKey)
        {
            return KeyFabrique.CréeKeyUIdRNo(texteKey);
        }

        protected override AKeyBase CréeAKeyDeListe(string texteKey)
        {
            return KeyFabrique.CréeKeyUIdOuUIdRNo(texteKey);
        }

        public async Task<IActionResult> DernierNo(string texteKey)
        {
            KeyUId aKey = KeyFabrique.CréeKeyUId(texteKey);
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