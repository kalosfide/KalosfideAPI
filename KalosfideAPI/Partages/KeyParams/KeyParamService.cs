using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyParams
{

    public abstract class KeyParamService<T, TVue, TParam> : BaseService<T, TVue>, IKeyParamService<T, TVue, TParam> where T : AKeyBase where TVue : AKeyBase where TParam : KeyParam
    {
        public abstract T NouvelleDonnée();
        public abstract void FixeKey(T donnée, TVue vue);
        public abstract void FixeVueKey(T donnée, TVue vue);

        protected DbSet<T> _dbSet;
        protected InclutRelations<T> _inclutRelations = null;
        protected Transforme<T, TVue> _transforme;

        public ValideFiltre<TVue> ValideVue { get; set; }

        public KeyParamService(ApplicationContext context) : base(context)
        {
        }

        public override T CréeDonnée(TVue vue)
        {
            T donnée = NouvelleDonnée();
            FixeKey(donnée, vue);
            CopieVueDansDonnées(donnée, vue);
            return donnée;
        }

        public virtual Task ValideAjoute(T donnée, ModelStateDictionary modelState)
        {
            return Task.CompletedTask;
        }

        public virtual void AjouteSansSauver(T donnée)
        {
            _dbSet.Add(donnée);
        }

        public async Task<RetourDeService<T>> Ajoute(T donnée)
        {
            AjouteSansSauver(donnée);
            return await SaveChangesAsync(donnée);
        }

        protected virtual void EditeSansSauver(T donnée)
        {
            _context.Update(donnée);
        }

        public virtual Task ValideEdite(T donnée, ModelStateDictionary modelState)
        {
            return Task.CompletedTask;
        }

        public async Task<RetourDeService<T>> Edite(T donnée)
        {
            EditeSansSauver(donnée);
            return await SaveChangesAsync(donnée);
        }

        protected void SupprimeSansSauver(T donnée)
        {
            _context.Remove(donnée);
        }

        public async Task<RetourDeService<T>> Supprime(T donnée)
        {
            SupprimeSansSauver(donnée);
            return await SaveChangesAsync(donnée);
        }

        public abstract Task<T> Lit(TParam param);
        protected abstract ValideFiltre<T> ValideFiltre(KeyParam param);

        protected async Task<List<TVue>> CréeVues(ValideFiltre<T> valideT, ValideFiltre<TVue> valideVue)
        {
            IQueryable<T> ts = valideT == null ? _dbSet : _dbSet.Where(t => valideT(t));
            IQueryable<T> tsComplets = _inclutRelations == null ? ts : _inclutRelations(ts);
            List<T> données = await tsComplets.ToListAsync();
            var vues = données.Select(t => (_transforme ?? CréeVue)(t));
            vues = valideVue == null ? vues : vues.Where(v => valideVue(v));
            return vues.ToList();
        }

        public async Task<TVue> LitVue(TParam param)
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
