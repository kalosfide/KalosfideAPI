using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using Microsoft.EntityFrameworkCore;

namespace KalosfideAPI.Partages.KeyParams
{
    public abstract class KeyUidRnoNoService<T> : KeyParamService<T, KeyParam>, IKeyUidRnoNoService<T> where T : AKeyUidRnoNo
    {
        public KeyUidRnoNoService(ApplicationContext context) : base(context)
        {
        }

        public override async Task<T> Lit(KeyParam param)
        {
            return await _dbSet.Where(entité => entité.Uid == param.Uid && entité.Rno == param.Rno && entité.No == param.No).FirstOrDefaultAsync();
        }

        public override async Task<List<T>> Liste(KeyParam param)
        {
            return param.Rno == null
                ? await _dbSet.Where(entité => entité.Uid == param.Uid).ToListAsync()
                : await _dbSet.Where(entité => entité.Uid == param.Uid && entité.Rno == param.Rno).ToListAsync();
        }

        public override async Task<List<T>> Liste()
        {
            List<T> liste = await _dbSet.ToListAsync();
            return liste;
        }

        public async Task<long> DernierNo(string uid, int rno)
        {
            var données = _dbSet.Where(donnée => donnée.Uid == uid && donnée.Rno == rno);
            return await données.AnyAsync() ? await données.MaxAsync(donnée => donnée.No) : 0;
        }
    }
}
