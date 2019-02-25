using KalosfideAPI.Data.Keys;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyParams
{
    /// <summary>
    /// ajoute au ModelState les éventuelles erreurs de la donnée
    /// </summary>
    /// <param name="donnée"></param>
    /// <param name="modelState"></param>
    /// <returns></returns>
    public delegate Task DValideAjoute<T>(T donnée, ModelStateDictionary modelState) where T : AKeyBase;

    /// <summary>
    /// ajoute au ModelState les éventuelles erreurs de la donnée
    /// </summary>
    /// <param name="donnée"></param>
    /// <param name="modelState"></param>
    /// <returns></returns>
    public delegate Task DValideEdite<T>(T donnée, ModelStateDictionary modelState) where T : AKeyBase;

    /// <summary>
    /// ajoute une erreur au ModelState la donnée n'est pas supprimable
    /// </summary>
    /// <param name="donnée"></param>
    /// <param name="modelState"></param>
    /// <returns></returns>
    public delegate Task DValideSupprime<T>(T donnée, ModelStateDictionary modelState) where T : AKeyBase;

    public interface IKeyParamService<T, TVue, TParam> : IBaseService<T, TVue> where T: AKeyBase where TVue: AKeyBase where TParam: KeyParam
    {
        DValideAjoute<T> DValideAjoute();
        DValideEdite<T> DValideEdite();
        DValideSupprime<T> DValideSupprime();

        Task<List<TVue>> CréeVuesAsync(List<T> données);
        void AjouteSansSauver(T donnée);
        Task<RetourDeService<T>> Ajoute(T donnée);
        Task<RetourDeService<T>> Edite(T donnée, T nouveau);
        Task<RetourDeService<T>> Supprime(T donnée);
        Task<T> Lit(TParam param);
        Task<TVue> LitVue(TParam param);
        Task<List<TVue>> Liste(KeyParam param);
        Task<List<TVue>> Liste();
        Task<List<TVue>> Liste(KeyParam param, ValideFiltre<TVue> valide);
        Task<List<TVue>> Liste(ValideFiltre<TVue> valide);
    }

    public interface IKeyParamService1<T, TVue, TParam> : IBaseService<T, TVue> where T: AKeyBase where TVue: AKeyBase where TParam: KeyParam
    {
        DValideAjoute<T> DValideAjoute();
        DValideEdite<T> DValideEdite();
        DValideSupprime<T> DValideSupprime();

        Task<List<TVue>> CréeVuesAsync(List<T> données);
        void AjouteSansSauver(T donnée);
        Task<RetourDeService<T>> Ajoute(T donnée);
        Task<RetourDeService<T>> Edite(T donnée, TVue vue);
        Task<RetourDeService<T>> Supprime(T donnée);
        Task<T> Lit(TParam param);
        Task<TVue> LitVue(TParam param);
        Task<List<TVue>> Liste(KeyParam param);
        Task<List<TVue>> Liste();
        Task<List<TVue>> Liste(KeyParam param, ValideFiltre<TVue> valide);
        Task<List<TVue>> Liste(ValideFiltre<TVue> valide);
    }
}
