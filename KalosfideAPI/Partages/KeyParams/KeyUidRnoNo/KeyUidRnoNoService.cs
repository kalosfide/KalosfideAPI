using System.Linq;
using System.Threading.Tasks;
using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using Microsoft.EntityFrameworkCore;

namespace KalosfideAPI.Partages.KeyParams
{
    public abstract class KeyUidRnoNoService<T, TVue, TEtat> : KeyParamService<T, TVue, TEtat, KeyParam>, IKeyUidRnoNoService<T, TVue>
       where T : AKeyUidRnoNo where TVue : AKeyUidRnoNo where TEtat : AKeyUidRnoNo
    {
        public KeyUidRnoNoService(ApplicationContext context) : base(context)
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
            }
            return v;
        }

        public async Task<long> DernierNo(KeyParam param)
        {
            var données = _dbSet.Where(donnée => donnée.Uid == param.Uid && donnée.Rno == param.Rno);
            return await données.AnyAsync() ? await données.MaxAsync(donnée => donnée.No) : 0;
        }
    }
}
