using KalosfideAPI.Data.Keys;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyParams
{
    public interface IKeyUidRnoService<T> : IKeyParamService<T, KeyParam> where T: AKeyUidRno
    {
        Task<int> DernierNo(string uid);
    }
}
