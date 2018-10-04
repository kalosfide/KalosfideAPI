using KalosfideAPI.Data;
using KalosfideAPI.Partages;
using KalosfideAPI.Sécurité;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KalosfideAPI.Utilisateurs
{
    public interface IUtilisateurService: IBaseService<Utilisateur>
    {
        Task<Utilisateur> UtilisateurAvecListeRoles(ApplicationUser applicationUser);

        Task<Utilisateur> UtilisateurAvecRoleSelectionné(ApplicationUser applicationUser);

        Task<bool> NomUnique(string nom);

        Task<RetourDeService<Utilisateur>> CréeUtilisateur(ApplicationUser applicationUser, string password);

        void ChangeRoleSansSauver(Utilisateur utilisateur, Role role);
        Task<RetourDeService<Utilisateur>> ChangeRole(Utilisateur utilisateur, Role role);

        Task<Utilisateur> Lit(string id);

        Task<List<Utilisateur>> Lit();

        Task<Utilisateur> UtilisateurDeUser(string userId);
        Task<bool> PeutAjouterRole(Utilisateur utilisateur, Client client);
        Task<bool> PeutAjouterRole(Utilisateur utilisateur, Fournisseur fournisseur);
}
}
