using KalosfideAPI.Data;
using KalosfideAPI.Erreurs;
using Microsoft.EntityFrameworkCore;
using System;
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
