using KalosfideAPI.Data;
using System.Collections.Generic;

namespace KalosfideAPI.Roles
{
    public class RoleTransformation: IRoleTransformation
    {
        public RoleVue CréeVue(Role role)
        {
            RoleVue vue = new RoleVue
            {
                UtilisateurId= role.UtilisateurId,
                No= role.No,
                Type = role.Type,
                Nom = role.Nom,
                Adresse = role.Adresse,
            };
            if (role.FournisseurId != null)
            {
                vue.FournisseurId = role.FournisseurId;
                vue.FournisseurNo = role.FournisseurNo;
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
            Role role = new Role
            {
                UtilisateurId = vue.UtilisateurId,
                No = vue.No,
                Type = vue.Type,
                Nom = vue.Nom,
                Adresse = vue.Adresse,
            };
            if (vue.FournisseurId != null)
            {
                role.FournisseurId = vue.FournisseurId;
                role.FournisseurNo = vue.FournisseurNo;
            }
            return role;
        }

        public void CopieVueDansDonnées(Role role, RoleVue vue)
        {
            role.Nom = vue.Nom;
            role.Adresse = vue.Adresse;
            if (vue.FournisseurId != null)
            {
                role.FournisseurId = vue.FournisseurId;
                role.FournisseurNo = vue.FournisseurNo;
            }
        }
    }
}
