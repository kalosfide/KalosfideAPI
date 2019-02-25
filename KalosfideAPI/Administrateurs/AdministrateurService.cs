using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Partages.KeyParams;
using System.Threading.Tasks;

namespace KalosfideAPI.Administrateurs
{
    public class AdministrateurService : KeyUidRnoService<Administrateur, AdministrateurVue, KeyUidRno>, IAdministrateurService
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
            vue.CopieKey(donnée.KeyParam);
            return vue;
        }
    }
}
