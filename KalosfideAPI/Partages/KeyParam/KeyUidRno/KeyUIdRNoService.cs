using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyParams
{
    public abstract class KeyUidRnoService<T> : KeyParamService<T, KeyParam>, IKeyUidRnoService<T> where T : AKeyUidRno
    {
        public KeyUidRnoService(ApplicationContext context) : base(context)
        {
        }

        public override async Task<T> Lit(KeyParam param)
        {
            return await _dbSet.Where(entité => entité.Uid == param.Uid && entité.Rno == param.Rno).FirstOrDefaultAsync();
        }

        public override async Task<List<T>> Liste(KeyParam param)
        {
            return await _dbSet.Where(entité => entité.Uid == param.Uid).ToListAsync();
        }

        public override async Task<List<T>> Liste()
        {
            List<T> liste = await _dbSet.ToListAsync();
            return liste;
        }

        public async Task<int> DernierNo(string uid)
        {
            var données = _dbSet.Where(donnée => donnée.Uid == uid);
            return await données.AnyAsync() ? await données.MaxAsync(donnée => donnée.Rno) : 0;
        }

    }
}
