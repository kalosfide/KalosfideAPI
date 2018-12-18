using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using Microsoft.EntityFrameworkCore;

namespace KalosfideAPI.Partages.KeyParams
{
    public abstract class KeyUidRnoNoService<T, TVue> : KeyParamService<T, TVue, KeyParam>, IKeyUidRnoNoService<T, TVue> where T : AKeyUidRnoNo where TVue : AKeyUidRnoNo
    {
        public KeyUidRnoNoService(ApplicationContext context) : base(context)
        {
        }

        public override void FixeKey(T donnée, TVue vue)
        {
            donnée.Uid = vue.Uid;
            donnée.Rno = vue.Rno;
            donnée.No = vue.No;
        }
        public override void FixeVueKey(T donnée, TVue vue)
        {
            vue.Uid = donnée.Uid;
            vue.Rno = donnée.Rno;
            vue.No = donnée.No;
        }

        public override async Task<T> Lit(KeyParam param)
        {
            return await _dbSet.Where(entité => entité.Uid == param.Uid && entité.Rno == param.Rno && entité.No == param.No).FirstOrDefaultAsync();
        }

        protected override ValideFiltre<T> ValideFiltre(KeyParam param)
        {
            ValideFiltre<T> v = null;
            if (param != null)
            {
                v = (T entité) => entité.Uid == param.Uid;
                if (param.Rno != null)
                {
                    v += (T entité) => entité.Rno == param.Rno;
                }
            }
            return v;
        }

        public async Task<long> DernierNo(string uid, int rno)
        {
            var données = _dbSet.Where(donnée => donnée.Uid == uid && donnée.Rno == rno);
            return await données.AnyAsync() ? await données.MaxAsync(donnée => donnée.No) : 0;
        }
    }
}
