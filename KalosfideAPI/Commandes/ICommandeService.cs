using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Partages;
using KalosfideAPI.Partages.KeyParams;
using System.Threading.Tasks;

namespace KalosfideAPI.Commandes
{
    public interface ICommandeService: IKeyUidRnoNoService<Commande, CommandeVue>
    {
        Task<RetourDeService<Commande>> AjouteVue(CommandeVue vue);

        Task<Commande> CommandeOuverte(AKeyUidRno key);
    }
}
