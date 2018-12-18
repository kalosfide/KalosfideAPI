using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using Microsoft.EntityFrameworkCore;

namespace KalosfideAPI.Partages.KeyParams
{
    public abstract class KeyUidRnoNo2Service<T, TVue> : KeyParamService<T, TVue, KeyParam>, IKeyUidRnoNo2Service<T, TVue> where T : AKeyUidRnoNo2 where TVue: AKeyUidRnoNo2
    {
        public KeyUidRnoNo2Service(ApplicationContext context) : base(context)
        {
        }
        public override void FixeKey(T donnée, TVue vue)
        {
            donnée.Uid = vue.Uid;
            donnée.Rno = vue.Rno;
            donnée.No = vue.No;
            donnée.Uid2 = vue.Uid2;
            donnée.Rno2 = vue.Rno2;
            donnée.No2 = vue.No2;
        }
        public override void FixeVueKey(T donnée, TVue vue)
        {
            vue.Uid = donnée.Uid;
            vue.Rno = donnée.Rno;
            vue.No = donnée.No;
            vue.Uid2 = donnée.Uid2;
            vue.Rno2 = donnée.Rno2;
            vue.No2 = donnée.No2;
        }

        public KeyUidRnoNo Key2(AKeyUidRnoNo2 key)
        {
            return new KeyUidRnoNo
            {
                Uid = key.Uid2,
                Rno = key.Rno2,
                No = key.No
            };
        }
        public override async Task<T> Lit(KeyParam param)
        {
            return await _dbSet.Where(entité => entité.EstSemblable(param)).FirstOrDefaultAsync();
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
                    if (param.No != null)
                    {
                        v += (T entité) => entité.No == param.No;
                    }
                }
            }
            return v;
        }
    }
}
