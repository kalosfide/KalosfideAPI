using KalosfideAPI.Data.Keys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyParams
{
    public interface IKeyParamService<T, TParam> : IBaseService<T> where T: AKeyBase where TParam: KeyParam
    {
        void AjouteSansSauver(T donnée);
        Task<RetourDeService<T>> Ajoute(T donnée);
        Task<T> Lit(TParam param);
        Task<List<T>> Liste(KeyParam param);
        Task<List<T>> Liste();
        Task<RetourDeService<T>> Edite(T donnée);
        Task<RetourDeService<T>> Supprime(T donnée);
    }
}
