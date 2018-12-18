using Microsoft.Extensions.DependencyInjection;

namespace KalosfideAPI.Démarrage
{
    public class ServicesDeDonnées
    {
        public static void ConfigureServices(IServiceCollection services)
        {

            Utilisateurs.Initialisation.ConfigureServices(services);

            Roles.Initialisation.ConfigureServices(services);

            Sites.Initialisation.ConfigureServices(services);

            Administrateurs.Initialisation.ConfigureServices(services);

            Fournisseurs.Initialisation.ConfigureServices(services);

            Clients.Initialisation.ConfigureServices(services);

            Catégories.Initialisation.ConfigureServices(services);

            Produits.Initialisation.ConfigureServices(services);

            FixePrix.Initialisation.ConfigureServices(services);

            Commandes.Initialisation.ConfigureServices(services);
        }
    }
}
