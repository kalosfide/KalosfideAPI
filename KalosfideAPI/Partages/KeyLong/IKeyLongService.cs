using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyLong
{
    public interface IKeyLongService<T> : IBaseService<T> where T : class, IKeyLong
    {
        Task<RetourDeService<T>> Ajoute(T donnée);
        Task<T> Lit(KeyLong key);
        Task<List<T>> Lit();
    }
}
