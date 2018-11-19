using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyParams
{
    public abstract class KeyParamService<T, TParam> : BaseService<T>, IKeyParamService<T, TParam> where T: AKeyBase where TParam: KeyParam
    {
        protected DbSet<T> _dbSet;

        public KeyParamService(ApplicationContext context) : base(context)
        {
        }

        public void AjouteSansSauver(T donnée)
        {
            _dbSet.Add(donnée);
        }

        public async Task<RetourDeService<T>> Ajoute(T donnée)
        {
            AjouteSansSauver(donnée);
            return await SaveChangesAsync(donnée);
        }

        protected void EditeSansSauver(T donnée)
        {
            _context.Update(donnée);
        }

        public async Task<RetourDeService<T>> Edite(T donnée)
        {
            EditeSansSauver(donnée);
            return await SaveChangesAsync(donnée);
        }

        protected void SupprimeSansSauver(T donnée)
        {
            _context.Remove(donnée);
        }

        public async Task<RetourDeService<T>> Supprime(T donnée)
        {
            SupprimeSansSauver(donnée);
            return await SaveChangesAsync(donnée);
        }

        public abstract Task<T> Lit(TParam param);
        public abstract Task<List<T>> Liste(KeyParam param);
        public abstract Task<List<T>> Liste();
    }
}
