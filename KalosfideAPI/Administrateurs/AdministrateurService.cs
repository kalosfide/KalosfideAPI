using KalosfideAPI.Data;
using KalosfideAPI.Partages.KeyParams;

namespace KalosfideAPI.Administrateurs
{
    public class AdministrateurService : KeyUidRnoService<Administrateur>, IAdministrateurService
    {
        public AdministrateurService(ApplicationContext context) : base(context)
        {
            _dbSet = context.Administrateur;
        }
    }
}
