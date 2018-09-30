using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using System.Collections.Generic;

namespace KalosfideAPI.Roles
{
    public class RoleTransformation: IRoleTransformation
    {
        public RoleVue CréeVue(Role role)
        {
            RoleVue vue = new RoleVue
            {
                VueId = role.Key,
                Type = role.Type,
                Nom = role.Nom,
                Adresse = role.Adresse,
            };
            if (role.FournisseurId != null)
            {
                vue.FournisseurKey = KeyFabrique.CréeKey(role.FournisseurId, role.FournisseurRoleNo.ToString());
            }
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
            KeyFabrique fabrique = new KeyFabrique(vue.Key);
            Role role = new Role
            {
                UtilisateurId = fabrique.KeyUIdRNo.UtilisateurId,
                RoleNo = fabrique.KeyUIdRNo.RoleNo,
                Type = vue.Type,
                Nom = vue.Nom,
                Adresse = vue.Adresse,
            };
            if (vue.FournisseurKey != null)
            {
                fabrique = new KeyFabrique(vue.FournisseurKey);
                role.FournisseurId = fabrique.KeyUIdRNo.UtilisateurId;
                role.FournisseurRoleNo = fabrique.KeyUIdRNo.RoleNo;
            }
            return role;
        }

        public void CopieVueDansDonnées(Role role, RoleVue vue)
        {
            role.Nom = vue.Nom;
            role.Adresse = vue.Adresse;
        }
    }
}
