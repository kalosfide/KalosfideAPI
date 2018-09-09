using KalosfideAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages
{
    public interface IKeyUtilisateurIdNoService<T> : IBaseService<T> where T: class, IKeyUtilisateurIdNo
    {
        Task<BaseServiceRetour<T>> Ajoute(T donnée);
        Task<T> Lit(KeyUtilisateurIdNo key);
        Task<T> Lit(KeyUtilisateurIdNo key, IQueryable<T> iQuery);
        Task<List<T>> Lit(KeyUtilisateurId key);
        Task<List<T>> Lit(KeyUtilisateurId key, IQueryable<T> iQuery);
        Task<List<T>> Lit(IKeyUtilisateurId ikey);
        Task<List<T>> Lit(IKeyUtilisateurId ikey, IQueryable<T> iQuery);
        Task<int> DernierNo(KeyUtilisateurId key);
    }
}
