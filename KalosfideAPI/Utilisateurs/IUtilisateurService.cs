using KalosfideAPI.Data;
using KalosfideAPI.Enregistrement;
using KalosfideAPI.Partages;
using KalosfideAPI.Sécurité;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KalosfideAPI.Utilisateurs
{
    public interface IUtilisateurService : IBaseService<Utilisateur, Utilisateur>
    {
        Task<ApplicationUser> TrouveParNom(string userName);
        Task<ApplicationUser> TrouveParEmail(string eMail);
        Task<ApplicationUser> ApplicationUserVérifié(string userName, string password);

        Task Connecte(ApplicationUser user, bool persistant);
        Task Déconnecte();

        Task<CarteUtilisateur> CréeCarteUtilisateur(ApplicationUser applicationUser);

        Task<bool> NomUnique(string nom);

        Task<RetourDeService<Utilisateur>> CréeUtilisateur(ApplicationUser applicationUser, string password);

        Task<Utilisateur> Lit(string id);

        Task<List<Utilisateur>> Lit();

        Task<Utilisateur> UtilisateurDeUser(string userId);
        Task<bool> PeutAjouterRole(Utilisateur utilisateur, ClientVue client);
        Task<bool> PeutAjouterRole(Utilisateur utilisateur, Fournisseur fournisseur);

        Task<RetourDeService<Utilisateur>> Supprime(Utilisateur utilisateur);

        Task<bool> UserNamePris(string userName);
        Task<bool> EmailPris(string eMail);
    }
}