using KalosfideAPI.Data;
using KalosfideAPI.Partages.KeyParams;
using System.Threading.Tasks;

namespace KalosfideAPI.Administrateurs
{
    public class AdministrateurService : KeyUidRnoService<Administrateur, AdministrateurVue>, IAdministrateurService
    {
        public AdministrateurService(ApplicationContext context) : base(context)
        {
            _dbSet = context.Administrateur;
        }

        public override Task CopieVueDansDonnées(Administrateur donnée, AdministrateurVue vue)
        {
            return Task.CompletedTask;
        }
        public override Administrateur NouvelleDonnée()
        {
            return new Administrateur();
        }

        public override AdministrateurVue CréeVue(Administrateur donnée)
        {
            AdministrateurVue vue = new AdministrateurVue();
            FixeVueKey(donnée, vue);
            return vue;
        }
    }
}
