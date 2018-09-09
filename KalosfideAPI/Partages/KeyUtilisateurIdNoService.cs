using KalosfideAPI.Data;
using KalosfideAPI.Erreurs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages
{
    public class KeyUtilisateurIdNoService<T> : BaseService<T>, IKeyUtilisateurIdNoService<T> where T : class, IKeyUtilisateurIdNo
    {
        private readonly DbSet<T> _dbSet;

        public KeyUtilisateurIdNoService(
            ApplicationContext context,
            DbSet<T> dbSet
            ) : base(context)
        {
            _dbSet = dbSet;
        }

        public async Task<BaseServiceRetour<T>> Ajoute(T donnée)
        {
            _dbSet.Add(donnée);
            try
            {
                await _context.SaveChangesAsync();
                return new BaseServiceRetour<T>(donnée);
            }
            catch (DbUpdateConcurrencyException)
            {
                return new BaseServiceRetour<T>(BaseServiceRetourType.ConcurrencyError);
            }
            catch (Exception)
            {
                return new BaseServiceRetour<T>(BaseServiceRetourType.Indéterminé);
            }
        }

        public async Task<T> Lit(KeyUtilisateurIdNo key, IQueryable<T> iQuery)
        {
            return await iQuery
                .Where(donnée => donnée.UtilisateurId == key.UtilisateurId && donnée.No == key.No)
                .FirstOrDefaultAsync();
        }

        public async Task<T> Lit(KeyUtilisateurIdNo key)
        {
            return await Lit(key, _dbSet);
        }

        public async Task<List<T>> Lit(KeyUtilisateurId key, IQueryable<T> iQuery)
        {
            return await iQuery.Where(donnée => donnée.UtilisateurId == key.UtilisateurId).ToListAsync();
        }

        public async Task<List<T>> Lit(KeyUtilisateurId key)
        {
            return await Lit(key, _dbSet);
        }

        public async Task<List<T>> Lit(IKeyUtilisateurId ikey, IQueryable<T> iQuery)
        {
            if (ikey is IKeyUtilisateurId)
            {
                return await Lit(ikey as IKeyUtilisateurId, iQuery);
            }
            if (ikey is IKeyUtilisateurIdNo)
            {
                return await Lit(ikey as IKeyUtilisateurIdNo, iQuery);
            }
            return null;
        }

        public async Task<List<T>> Lit(IKeyUtilisateurId ikey)
        {
            return await Lit(ikey, _dbSet);
        }

        public async Task<BaseServiceRetour<T>> Edite(T donnée)
        {
            _context.Update(donnée);
            try
            {
                await _context.SaveChangesAsync();
                return new BaseServiceRetour<T>(donnée);
            }
            catch (DbUpdateConcurrencyException)
            {
                return new BaseServiceRetour<T>(BaseServiceRetourType.ConcurrencyError);
            }
            catch (Exception)
            {
                return new BaseServiceRetour<T>(BaseServiceRetourType.Indéterminé);
            }
        }

        public async Task<BaseServiceRetour<T>> Supprime(T donnée)
        {
            _context.Remove(donnée);
            try
            {
                await _context.SaveChangesAsync();
                return new BaseServiceRetour<T>(donnée);
            }
            catch (DbUpdateConcurrencyException)
            {
                return new BaseServiceRetour<T>(BaseServiceRetourType.ConcurrencyError);
            }
            catch (Exception)
            {
                return new BaseServiceRetour<T>(BaseServiceRetourType.Indéterminé);
            }
        }

        public async Task<int> DernierNo(KeyUtilisateurId key)
        {
            return await _dbSet.Where(donnée => donnée.UtilisateurId == key.UtilisateurId).MaxAsync(donnée => donnée.No);
        }

    }
}
