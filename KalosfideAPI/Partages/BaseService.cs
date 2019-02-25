using KalosfideAPI.Data;
using KalosfideAPI.Erreurs;
using KalosfideAPI.Partages.KeyParams;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages
{
    public delegate Task<ErreurDeModel> DValidationAsync<T>(T donnée) where T : class;
    public delegate bool ValideFiltre<T>(T t);
    public delegate Task<TVue> Transforme<T, TVue>(T t);
    public delegate IQueryable<T> InclutRelations<T>(IQueryable<T> queryT);

    public abstract class BaseService<T, TVue> : IBaseService<T, TVue> where T : class where TVue : class
    {
        public DValidationAsync<T> DValidation;
        protected ApplicationContext _context;
        public abstract TVue CréeVue(T donnée);

        public abstract T CréeDonnée(TVue vue);
        public abstract Task CopieVueDansDonnées(T donnée, TVue vue);

        protected BaseService(ApplicationContext context) { _context = context; }

        public async Task<ErreurDeModel> Validation(T donnée)
        {
            if (DValidation != null)
            {
                return await DValidation(donnée);
            }
            return null;
        }

        public async Task<RetourDeService> SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return new RetourDeService(TypeRetourDeService.Ok);
            }
            catch (DbUpdateConcurrencyException)
            {
                return new RetourDeService<T>(TypeRetourDeService.ConcurrencyError);
            }
            catch (Exception)
            {
                return new RetourDeService<T>(TypeRetourDeService.Indéterminé);
            }
        }

        public async Task<RetourDeService<T>> SaveChangesAsync(T donnée)
        {
            try
            {
                await _context.SaveChangesAsync();
                return new RetourDeService<T>(donnée);
            }
            catch (DbUpdateConcurrencyException)
            {
                return new RetourDeService<T>(TypeRetourDeService.ConcurrencyError);
            }
            catch (Exception)
            {
                return new RetourDeService<T>(TypeRetourDeService.Indéterminé);
            }
        }
    }
}
