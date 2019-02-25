using System.Linq;
using System.Threading.Tasks;
using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using Microsoft.EntityFrameworkCore;

namespace KalosfideAPI.Partages.KeyParams
{
    public abstract class KeyUidRnoNo2Service<T, TVue, TEtat> : KeyParamService<T, TVue, TEtat, KeyParam>, IKeyUidRnoNo2Service<T, TVue>
       where T : AKeyUidRnoNo2 where TVue : AKeyUidRnoNo2 where TEtat : AKeyUidRnoNo2
    {
        public KeyUidRnoNo2Service(ApplicationContext context) : base(context)
        {
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
