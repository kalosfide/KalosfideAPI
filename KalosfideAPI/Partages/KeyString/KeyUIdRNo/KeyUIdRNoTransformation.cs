using KalosfideAPI.Data.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyString
{
    public class KeyUIdRNoTransformation<T, TVue> : KeyStringTransformation<T, TVue> where T : AKeyUIdRNo where TVue : AKeyUIdRNo
    {
        public override void FixeKey(T donnée, TVue vue)
        {
            donnée.UtilisateurId = vue.UtilisateurId;
            donnée.RoleNo = vue.RoleNo;
        }
        public override void FixeVueId(T donnée, TVue vue)
        {
            vue.UtilisateurId = donnée.UtilisateurId;
            vue.RoleNo = donnée.RoleNo;
        }
    }
}
