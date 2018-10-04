using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Enregistrement;
using KalosfideAPI.Roles;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyString
{
    public class KeyRIdController<T, TVue> : KeyStringController<T, TVue>
        where T : AKeyRId where TVue : AKeyRId
    {
        protected readonly IRoleService _roleService;

        protected IKeyRIdService<T> _service { get { return __service as IKeyRIdService<T>; } }
        protected IKeyRIdTransformation<T, TVue> _transformation { get { return __transformation as IKeyRIdTransformation<T, TVue>; } }

        public KeyRIdController(
            IRoleService roleService,
            IKeyRIdService<T> service,
            IKeyRIdTransformation<T, TVue> transformation
            ) : base(service, transformation)
        {
            _roleService = roleService;
        }

        protected override AKeyBase CréeAKey(string texteKey)
        {
            return KeyFabrique.CréeKeyRId(texteKey);
        }

        protected override AKeyBase CréeAKeyDeListe(string texteKey)
        {
            return KeyFabrique.CréeKeyRIdOuRIdNo(texteKey);
        }
    }
}