using KalosfideAPI.Data.Keys;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyString
{
    public class KeyRIdNoController<T, TVue> : KeyStringController<T, TVue>
        where T : AKeyRIdNo where TVue : AKeyRIdNo
    {
        protected IKeyRIdNoService<T> _service { get { return __service as IKeyRIdNoService<T>; } }
        protected IKeyRIdNoTransformation<T, TVue> _transformation { get { return __transformation as IKeyRIdNoTransformation<T,TVue>; } }

        public KeyRIdNoController(
            IKeyRIdNoService<T> service,
            IKeyRIdNoTransformation<T, TVue> transformation
            ) : base(service, transformation)
        {
            opérations.Add(new Opération { Nom = nameof(DernierNo) });
        }

        protected override AKeyBase CréeAKey(string texteKey)
        {
            return KeyFabrique.CréeKeyRIdNo(texteKey);
        }

        protected override AKeyBase CréeAKeyDeListe(string texteKey)
        {
            return KeyFabrique.CréeKeyRIdOuRIdNo(texteKey);
        }

        public async Task<IActionResult> DernierNo(string texteKey)
        {
            KeyRIdNo aKey = KeyFabrique.CréeKeyRIdNo(texteKey);
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
