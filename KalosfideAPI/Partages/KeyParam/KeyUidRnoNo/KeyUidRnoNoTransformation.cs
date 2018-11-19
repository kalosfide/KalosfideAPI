using KalosfideAPI.Data.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyParams
{
    public class KeyUidRnoNoTransformation<T, TVue>: KeyParamTransformation<T, TVue> where T : AKeyUidRnoNo where TVue : AKeyUidRnoNo
    {
        public override void FixeKey(T donnée, TVue vue)
        {
            donnée.Uid = vue.Uid;
            donnée.Rno = vue.Rno;
            donnée.No = vue.No;
        }
        public override void FixeVueId(T donnée, TVue vue)
        {
            vue.Uid = donnée.Uid;
            vue.Rno = donnée.Rno;
            vue.No = donnée.No;
        }
    }
}
