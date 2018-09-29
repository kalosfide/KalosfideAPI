using KalosfideAPI.Data.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyString
{
    public interface IKeyStringService<T> : IBaseService<T> where T: AKeyString
    {
        Task<RetourDeService<T>> Ajoute(T donnée);
        Task<T> Lit(AKeyString key);
        Task<List<T>> Liste(AKeyString key);
        Task<List<T>> Liste();
        Task<List<T>> ListeAOptions(AKeyString key, OptionsDeListe options);
        Task<List<T>> ListeAOptions(OptionsDeListe options);
    }
}
