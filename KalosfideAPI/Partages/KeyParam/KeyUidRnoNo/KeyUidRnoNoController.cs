using KalosfideAPI.Data.Keys;
using KalosfideAPI.Erreurs;
using KalosfideAPI.Sécurité;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyParams
{
    public class KeyUidRnoNoController<T, TVue> : KeyParamController<T, TVue, KeyParam> where T : AKeyUidRnoNo where TVue : AKeyUidRnoNo
    {

        public KeyUidRnoNoController(
            IKeyUidRnoNoService<T> service,
            IKeyUidRnoNoTransformation<T, TVue> transformation
            ) : base(service, transformation)
        {
        }

        private IKeyUidRnoNoService<T> _service { get => __service as IKeyUidRnoNoService<T>; }

        protected async override Task FixeKeyParamAjout(TVue vue)
        {
            vue.No = await _service.DernierNo(vue.Uid, vue.Rno) + 1;
        }

    }
}
