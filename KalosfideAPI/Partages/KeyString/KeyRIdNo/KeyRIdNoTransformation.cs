using KalosfideAPI.Data.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyString
{
    public class KeyRIdNoTransformation<T, TVue>: KeyStringTransformation<T, TVue> where T : AKeyRIdNo where TVue : AKeyRIdNo
    {
        public override void FixeKey(T donnée, TVue vue)
        {
            donnée.RoleId = vue.RoleId;
            donnée.No = vue.No;
        }
        public override void FixeVueId(T donnée, TVue vue)
        {
            vue.RoleId = donnée.RoleId;
            vue.No = donnée.No;
        }
    }
}
