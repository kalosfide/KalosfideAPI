using KalosfideAPI.Data.Keys;
using KalosfideAPI.Erreurs;
using KalosfideAPI.Sécurité;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyParams
{
    public abstract class KeyUidRnoController<T, TVue> : KeyParamController<T, TVue, KeyParam> where T : AKeyUidRno where TVue : AKeyUidRno
    {

        public KeyUidRnoController(
            IKeyUidRnoService<T> service,
            IKeyUidRnoTransformation<T, TVue> transformation
            ) : base(service, transformation)
        {
        }

        private IKeyUidRnoService<T> _service { get => __service as IKeyUidRnoService<T>; }

        protected async override Task FixeKeyParamAjout(TVue vue)
        {
            vue.Rno = await _service.DernierNo(vue.Uid) + 1;
        }
    }
}