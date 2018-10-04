using KalosfideAPI.Data.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyString
{
    public interface IKeyStringService<T> : IBaseService<T> where T: AKeyBase
    {
        void AjouteSansSauver(T donnée);
        Task<RetourDeService<T>> Ajoute(T donnée);
        Task<T> Lit(AKeyBase key);
        Task<List<T>> Liste(AKeyBase key);
        Task<List<T>> Liste();
        Task<List<T>> ListeAOptions(AKeyBase key, OptionsDeListe options);
        Task<List<T>> ListeAOptions(OptionsDeListe options);
    }
}
