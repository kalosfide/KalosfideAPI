using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyParams
{

    public abstract class GéreEtat<T, TVue, TEtat> where T : AKeyBase where TVue : AKeyBase where TEtat : AKeyBase
    {

        protected DbSet<TEtat> _dbSetEtat;

        public GéreEtat(DbSet<TEtat> dbSetEtat)
        {
            _dbSetEtat = dbSetEtat;
        }

        /// <summary>
        /// crée un TEtat sans clé reprenant tous les champs de donnée sauf la clé et la Date
        /// </summary>
        /// <param name="donnée"></param>
        /// <returns></returns>
        protected abstract TEtat CréeEtatAjout(T donnée);

        public void AjouteEtatAjout(T donnée)
        {
            TEtat etat = CréeEtatAjout(donnée);
            etat.CopieKey(donnée.KeyParam);
            _dbSetEtat.Add(etat);
        }

        /// <summary>
        /// crée un TEtat sans clé reprenant tous les champs de vue qui diffèrent des champs correspondants de donnée
        /// met à jour ces champs dans donnée
        /// </summary>
        /// <param name="donnée"></param>
        /// <returns>null si rien n'a changé</returns>
        protected abstract TEtat CréeEtatEdite(T donnée, TVue vue);

        public void AjouteEtatEdite(T donnée, TVue vue)
        {
            TEtat etat = CréeEtatEdite(donnée, vue);
            if (etat != null)
            {
                _dbSetEtat.Add(etat);
            }
        }

        public async Task SupprimeEtats(T donnée)
        {
            _dbSetEtat.RemoveRange(await _dbSetEtat
                .Where(ep => donnée.EstSemblable(ep))
                .ToListAsync());
        }

    }

    public abstract class KeyParamService<T, TVue, TEtat, TParam> : BaseService<T, TVue>, IKeyParamService<T, TVue, TParam>
        where T : AKeyBase where TVue : AKeyBase where TEtat : AKeyBase where TParam : KeyParam
    {

        protected DbSet<T> _dbSet;
        protected GéreEtat<T, TVue, TEtat> _géreEtat;

        protected DValideAjoute<T> dValideAjoute;
        public DValideAjoute<T> DValideAjoute()
        {
            return dValideAjoute;
        }
        protected DValideEdite<T> dValideEdite;
        public DValideEdite<T> DValideEdite()
        {
            return dValideEdite;
        }
        protected DValideSupprime<T> dValideSupprime;
        public DValideSupprime<T> DValideSupprime()
        {
            return dValideSupprime;
        }
    
        public abstract T NouvelleDonnée();

        protected InclutRelations<T> _inclutRelations = null;
        protected Transforme<T, TVue> CréeVueAsync;

        public ValideFiltre<TVue> ValideVue { get; set; }

        public KeyParamService(ApplicationContext context) : base(context)
        {
        }

        public override T CréeDonnée(TVue vue)
        {
            T donnée = NouvelleDonnée();
            vue.CopieKey(donnée.KeyParam);
            CopieVueDansDonnées(donnée, vue);
            return donnée;
        }

        public void AjouteSansSauver(T donnée)
        {
            _dbSet.Add(donnée);
            if (_géreEtat != null)
            {
                _géreEtat.AjouteEtatAjout(donnée);
            }
        }

        public async Task<RetourDeService<T>> Ajoute(T donnée)
        {
            AjouteSansSauver(donnée);
            return await SaveChangesAsync(donnée);
        }

        public void EditeSansSauver(T donnée, TVue vue)
        {
            if (_géreEtat != null)
            {
                _géreEtat.AjouteEtatEdite(donnée, vue);
            }
            else
            {
                CopieVueDansDonnées(donnée, vue);
            }
            _dbSet.Update(donnée);
        }

        public async Task<RetourDeService<T>> Edite(T donnée, TVue vue)
        {
            EditeSansSauver(donnée, vue);
            return await SaveChangesAsync(donnée);
        }

        public async Task SupprimeSansSauver(T donnée)
        {
            _dbSet.Remove(donnée);
            if (_géreEtat != null)
            {
                await _géreEtat.SupprimeEtats(donnée);
            }
        }

        public async Task<RetourDeService<T>> Supprime(T donnée)
        {
            await SupprimeSansSauver(donnée);
            return await SaveChangesAsync(donnée);
        }

        public abstract Task<T> Lit(TParam param);
        protected abstract ValideFiltre<T> ValideFiltre(KeyParam param);

        public async Task<List<TVue>> CréeVuesAsync(List<T> données)
        {
            return CréeVueAsync == null
                ? données.Select(t => CréeVue(t)).ToList()
                : (await Task.WhenAll(données.Select(t => CréeVueAsync(t)))).ToList();
        }

        protected virtual async Task<List<TVue>> CréeVues(ValideFiltre<T> valideT, ValideFiltre<TVue> valideVue)
        {
            IQueryable<T> ts = valideT == null ? _dbSet : _dbSet.Where(t => valideT(t));
            IQueryable<T> tsComplets = _inclutRelations == null ? ts : _inclutRelations(ts);
            List<T> données = await tsComplets.ToListAsync();

            List<TVue> vues = await CréeVuesAsync(données);

            vues = valideVue == null ? vues : vues.Where(v => valideVue(v)).ToList();
            return vues;
        }

        public virtual async Task<TVue> LitVue(TParam param)
        {
            List<TVue> vues = await CréeVues((T t) => t.EstSemblable(param), null);
            return vues.Count > 0 ? vues[0] : null;
        }
        public async Task<List<TVue>> Liste(KeyParam param)
        {
            return await CréeVues(ValideFiltre(param), null);
        }

        public async Task<List<TVue>> Liste()
        {
            return await CréeVues(null, null);
        }

        public async Task<List<TVue>> Liste(KeyParam param, ValideFiltre<TVue> valide)
        {
            return await CréeVues(ValideFiltre(param), valide);
        }

        public async Task<List<TVue>> Liste(ValideFiltre<TVue> valide)
        {
            return await CréeVues(null, valide);
        }

    }
}
