using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using Microsoft.EntityFrameworkCore;

namespace KalosfideAPI.Partages.KeyString.KeyUIdRNoNo
{
    public abstract class KeyUIdRNoNoService<T> : KeyStringService<T>, IKeyUIdRNoNoService<T> where T : AKeyUIdRNoNo
    {
        public KeyUIdRNoNoService(ApplicationContext context, DbSet<T> dbSet) : base(context, dbSet)
        {
        }

        public async Task<long> DernierNo(AKeyUIdRNo key)
        {
            var données = _dbSet.Where(donnée => donnée.RoleKey == key.RoleKey);
            return await données.AnyAsync() ? await données.MaxAsync(donnée => donnée.No) : 0;
        }
    }
}
