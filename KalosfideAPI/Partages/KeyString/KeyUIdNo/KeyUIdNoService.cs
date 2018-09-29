using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyString
{
    public abstract class KeyUIdRNoService<T> : KeyStringService<T>, IKeyUIdRNoService<T> where T : AKeyUIdRNo
    {
        public KeyUIdRNoService(
            ApplicationContext context,
            DbSet<T> dbSet
            ) : base(context, dbSet)
        {
        }

        public async Task<T> Lit(KeyUIdRNo key, IQueryable<T> iQuery)
        {
            return await iQuery
                .Where(donnée => donnée.UtilisateurId == key.UtilisateurId && donnée.RoleNo == key.RoleNo)
                .FirstOrDefaultAsync();
        }

        public async Task<List<T>> Liste(KeyUId key, IQueryable<T> iQuery)
        {
            return await iQuery.Where(donnée => donnée.UtilisateurId == key.UtilisateurId).ToListAsync();
        }

        public async Task<List<T>> Liste(IQueryable<T> iQuery)
        {
            return await iQuery.ToListAsync();
        }

        public async Task<T> Lit(KeyUIdRNo key)
        {
            return await Lit(key, _dbSet);
        }

        public async Task<List<T>> Liste(KeyUId key)
        {
            return await Liste(key, _dbSet);
        }

        public async Task<int> DernierNo(KeyUId key)
        {
            var données = _dbSet.Where(donnée => donnée.UtilisateurId == key.UtilisateurId);
            return await données.AnyAsync() ? await données.MaxAsync(donnée => donnée.RoleNo) : 0;
        }

    }
}
