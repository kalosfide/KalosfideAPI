using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyParams
{
    public abstract class KeyUidRnoService<T, TVue, TEtat> : KeyParamService<T, TVue, TEtat, KeyParam>, IKeyUidRnoService<T, TVue>
        where T : AKeyUidRno where TVue: AKeyUidRno where TEtat: AKeyUidRno
    {
        public KeyUidRnoService(ApplicationContext context) : base(context)
        {
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
