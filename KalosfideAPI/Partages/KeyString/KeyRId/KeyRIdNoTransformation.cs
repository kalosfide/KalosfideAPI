using KalosfideAPI.Data.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyString
{
    public class KeyRIdTransformation<T, TVue>: KeyStringTransformation<T, TVue> where T : AKeyRId where TVue : AKeyRId
    {
        public override void FixeKey(T donnée, TVue vue)
        {
            donnée.RoleId = vue.RoleId;
        }
        public override void FixeVueId(T donnée, TVue vue)
        {
            vue.RoleId = donnée.RoleId;
        }
    }
}
