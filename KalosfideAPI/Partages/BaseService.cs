using KalosfideAPI.Data;
using KalosfideAPI.Erreurs;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages
{
    public delegate Task<ErreurDeModel> DValidation<T>(T donnée) where T : class;

    public class BaseService<T> where T : class
    {
        public DValidation<T> DValidation;
        protected ApplicationContext _context;

        protected BaseService(ApplicationContext context) { _context = context; }

        public async Task<ErreurDeModel> Validation(T donnée)
        {
            if (DValidation != null)
            {
                return await DValidation(donnée);
            }
            return null;
        }
    }
}
