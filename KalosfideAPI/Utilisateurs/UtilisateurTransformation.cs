using KalosfideAPI.Data;
using System.Collections.Generic;

namespace KalosfideAPI.Utilisateurs
{
    public class UtilisateurTransformation : IUtilisateurTransformation
    {
        public UtilisateurVue CréeVue(Utilisateur utilisateur)
        {
            return new UtilisateurVue
            {
                UserId = utilisateur.UserId,
                UtilisateurId = utilisateur.Uid,
                Nom = utilisateur.ApplicationUser.UserName,
                Email = utilisateur.ApplicationUser.Email,
            };
        }
        public IEnumerable<UtilisateurVue> CréeVues(IEnumerable<Utilisateur> utilisateurs)
        {
            List<UtilisateurVue> vues = new List<UtilisateurVue>();
            foreach (Utilisateur utilisateur in utilisateurs)
            {
                vues.Add(CréeVue(utilisateur));
            }
            return vues;
        }
        public Utilisateur CréeDonnée(UtilisateurVue utilisateurVue)
        {
            return new Utilisateur
            {
            };
        }
        public void CopieVueDansDonnées(Utilisateur utilisateur, UtilisateurVue utilisateurVue)
        {
            utilisateur.ApplicationUser.UserName = utilisateurVue.Nom;
            utilisateur.ApplicationUser.Email = utilisateurVue.Email;
        }
    }
}
