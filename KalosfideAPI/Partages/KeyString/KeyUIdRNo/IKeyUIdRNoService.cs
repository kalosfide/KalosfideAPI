using KalosfideAPI.Data.Keys;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyString
{
    public interface IKeyUIdRNoService<T> : IKeyStringService<T> where T: AKeyUIdRNo
    {
        Task<int> DernierNo(KeyUId key);
    }
}
