using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyParams
{
    public abstract class KeyUidRnoService<T, TVue> : KeyParamService<T, TVue, KeyParam>, IKeyUidRnoService<T, TVue> where T : AKeyUidRno where TVue: AKeyUidRno
    {
        public KeyUidRnoService(ApplicationContext context) : base(context)
        {
        }

        public override void FixeKey(T donnée, TVue vue)
        {
            donnée.Uid = vue.Uid;
            donnée.Rno = vue.Rno;
        }
        public override void FixeVueKey(T donnée, TVue vue)
        {
            vue.Uid = donnée.Uid;
            vue.Rno = donnée.Rno;
        }

        public override async Task<T> Lit(KeyParam param)
        {
            return await _dbSet.Where(entité => entité.Uid == param.Uid && entité.Rno == param.Rno).FirstOrDefaultAsync();
        }

        protected override ValideFiltre<T> ValideFiltre(KeyParam param)
        {
            ValideFiltre<T> v = null;
            if (param != null)
            {
                v = (T entité) => entité.Uid == param.Uid;
            }
            return v;
        }

        public async Task<int> DernierNo(string uid)
        {
            var données = _dbSet.Where(donnée => donnée.Uid == uid);
            return await données.AnyAsync() ? await données.MaxAsync(donnée => donnée.Rno) : 0;
        }

    }
}
