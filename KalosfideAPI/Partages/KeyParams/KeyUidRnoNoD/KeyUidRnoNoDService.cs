using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyParams
{
    public abstract class KeyUidRnoNoDService<T, TVue> : KeyParamService<T, TVue, KeyParam>, IKeyUidRnoNoDService<T, TVue> where T : AKeyUidRnoNoD where TVue : AKeyUidRnoNoD
    {
        public KeyUidRnoNoDService(ApplicationContext context) : base(context)
        {
        }

        public override void FixeKey(T donnée, TVue vue)
        {
            donnée.Uid = vue.Uid;
            donnée.Rno = vue.Rno;
            donnée.No = vue.No;
            donnée.Date = vue.Date;
        }
        public override void FixeVueKey(T donnée, TVue vue)
        {
            vue.Uid = donnée.Uid;
            vue.Rno = donnée.Rno;
            vue.No = donnée.No;
            vue.Date = donnée.Date;
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
                if (param.No != null)
                {
                    v += (T entité) => entité.No == param.No;
                }
            }
            return v;
        }
    }
}

