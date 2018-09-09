using Microsoft.Extensions.DependencyInjection;

namespace KalosfideAPI.Démarrage
{
    public class ServicesDeDonnées
    {
        public static void ConfigureServices(IServiceCollection services)

        {

            SiteInfos.Initialisation.ConfigureServices(services);

            Utilisateurs.Initialisation.ConfigureServices(services);

            Roles.Initialisation.ConfigureServices(services);

        }
    }
}
