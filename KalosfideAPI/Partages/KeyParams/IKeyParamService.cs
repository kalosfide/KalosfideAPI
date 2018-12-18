using KalosfideAPI.Data.Keys;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyParams
{
    public interface IKeyParamService<T, TVue, TParam> : IBaseService<T, TVue> where T: AKeyBase where TVue: AKeyBase where TParam: KeyParam
    {
        void AjouteSansSauver(T donnée);
        Task ValideAjoute(T donnée, ModelStateDictionary modelState);
        Task<RetourDeService<T>> Ajoute(T donnée);
        Task<T> Lit(TParam param);
        Task<TVue> LitVue(TParam param);
        Task<List<TVue>> Liste(KeyParam param);
        Task<List<TVue>> Liste();
        Task<List<TVue>> Liste(KeyParam param, ValideFiltre<TVue> valide);
        Task<List<TVue>> Liste(ValideFiltre<TVue> valide);
        Task ValideEdite(T donnée, ModelStateDictionary modelState);
        Task<RetourDeService<T>> Edite(T donnée);
        Task<RetourDeService<T>> Supprime(T donnée);
    }
}
