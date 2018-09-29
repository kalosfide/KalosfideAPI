using KalosfideAPI.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyLong
{
    public class KeyLongService<T> : BaseService<T>, IKeyLongService<T> where T : class, IKeyLong
    {
        private readonly DbSet<T> _dbSet;

        public KeyLongService(
            ApplicationContext context,
            DbSet<T> dbSet
            ) : base(context)
        {
            _dbSet = dbSet;
        }

        public async Task<RetourDeService<T>> Ajoute(T donnée)
        {
            _dbSet.Add(donnée);
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

        public async Task<T> Lit(KeyLong key)
        {
            return await _dbSet
                .Where(donnée => donnée.Id == key.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<T>> Lit()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<RetourDeService<T>> Edite(T donnée)
        {
            _context.Update(donnée);
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

        public async Task<RetourDeService<T>> Supprime(T donnée)
        {
            _context.Remove(donnée);
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
