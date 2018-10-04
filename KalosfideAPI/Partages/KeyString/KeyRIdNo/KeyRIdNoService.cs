using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using Microsoft.EntityFrameworkCore;

namespace KalosfideAPI.Partages.KeyString
{
    public abstract class KeyRIdNoService<T> : KeyStringService<T>, IKeyRIdNoService<T> where T : AKeyRIdNo
    {
        public KeyRIdNoService(ApplicationContext context, DbSet<T> dbSet) : base(context, dbSet)
        {
        }

        public async Task<long> DernierNo(AKeyRIdNo key)
        {
            var données = _dbSet.Where(donnée => donnée.RoleId == key.RoleId);
            return await données.AnyAsync() ? await données.MaxAsync(donnée => donnée.No) : 0;
        }
    }
}
