using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages
{
    public interface IKeyLongService<T> : IBaseService<T> where T : class, IKeyLong
    {
        Task<BaseServiceRetour<T>> Ajoute(T donnée);
        Task<T> Lit(KeyLong key);
        Task<List<T>> Lit();
    }
}
