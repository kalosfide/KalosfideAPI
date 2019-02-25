using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Partages;
using KalosfideAPI.Partages.KeyParams;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KalosfideAPI.Commandes
{
    public interface ICommandeService: IKeyUidRnoNoService<Commande, CommandeVue>
    {
        void CréePremièreCommande(AKeyUidRno keyClient, AKeyUidRno keySite);

        Task<CommandeVue> CommandeEnCours(AKeyUidRno keyClient);
        Task<RetourDeService> EnvoiBon(CommandeVue vue);

        Task<List<CommandeVue>> CommandesDeLivraison(AKeyUidRno keyFournisseur);

        Task<int> NbOuvertes(AKeyUidRno keyFournisseur);
        Task<List<CommandeVue>> VuesOuvertes(AKeyUidRno keyFournisseur);
        Task<List<Commande>> VérifieEnregistre(AKeyUidRno keyFournisseur, List<CommandeVue> vues);
        Task<RetourDeService> Enregistre(AKeyUidRno keyFournisseur, List<Commande> vérifiées);

        Task<RetourDeService> FermeOuvertes(AKeyUidRno keyFournisseur);

        Task<Commande> Ouverte(AKeyUidRno keyClient);
        Task<List<CommandeVue>> NouvellesCommandes(AKeyUidRno key);
    }
}
