using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Partages.KeyString;
using System.Collections.Generic;

namespace KalosfideAPI.Roles
{
    public class RoleTransformation: KeyUIdRNoTransformation<Role, RoleVue>, IRoleTransformation
    {
        public RoleVue CréeVue(Role role)
        {
            RoleVue vue = new RoleVue
            {
                Type = role.Type,
            };
            return vue;
        }

        public IEnumerable<RoleVue> CréeVues(IEnumerable<Role> roles)
        {
            List<RoleVue> vues = new List<RoleVue>();
            foreach (Role role in roles)
            {
                vues.Add(CréeVue(role));
            }
            return vues;
        }

        public Role CréeDonnée(RoleVue vue)
        {
            Role role = new Role
            {
                UtilisateurId = vue.UtilisateurId,
                RoleNo = vue.RoleNo,
            };
            role.FixeType(vue.Type);
            return role;
        }

        public void CopieVueDansDonnées(Role role, RoleVue vue)
        {
        }
    }
}
