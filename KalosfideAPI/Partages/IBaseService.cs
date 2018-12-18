using KalosfideAPI.Erreurs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages
{
    public interface IBaseService<T, TVue> where T : class where TVue : class
    {
        TVue CréeVue(T donnée);
        T CréeDonnée(TVue vue);
        Task CopieVueDansDonnées(T donnée, TVue vue);

        Task<ErreurDeModel> Validation(T donnée);
        Task<RetourDeService> SaveChangesAsync();
        Task<RetourDeService<T>> SaveChangesAsync(T donnée);
    }
}
