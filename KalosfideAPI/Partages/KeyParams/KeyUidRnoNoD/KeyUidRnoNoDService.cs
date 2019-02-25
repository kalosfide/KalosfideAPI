using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyParams
{
    public abstract class KeyUidRnoNoDService<T, TVue, TEtat> : KeyParamService<T, TVue, TEtat, KeyParam>, IKeyUidRnoNoDService<T, TVue>
       where T : AKeyUidRnoNoD where TVue : AKeyUidRnoNoD where TEtat : AKeyUidRnoNoD
    {
        public KeyUidRnoNoDService(ApplicationContext context) : base(context)
        {
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

