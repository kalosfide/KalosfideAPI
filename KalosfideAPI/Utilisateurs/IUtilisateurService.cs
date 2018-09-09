using KalosfideAPI.Data;
using KalosfideAPI.Partages;
using KalosfideAPI.Sécurité;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KalosfideAPI.Utilisateurs
{
    public interface IUtilisateurService
    {
        Task<Utilisateur> UtilisateurAvecListeRoles(ApplicationUser applicationUser);

        Task<Utilisateur> UtilisateurAvecRoleSelectionné(ApplicationUser applicationUser);

        Task<bool> NomUnique(string nom);

        Task<BaseServiceRetour<Utilisateur>> Enregistre(ApplicationUser applicationUser, string password);

        Task<BaseServiceRetour<Utilisateur>> ChangeRoleActif(Utilisateur utilisateur, Role role);

        Task<Utilisateur> Lit(string id);

        Task<List<Utilisateur>> Lit();

        Task<BaseServiceRetour<Utilisateur>> Update(Utilisateur utilisateur);
        Task<BaseServiceRetour<Utilisateur>> Delete(Utilisateur utilisateur);
    }
}
