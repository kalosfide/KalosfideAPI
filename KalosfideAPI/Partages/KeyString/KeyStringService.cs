using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyString
{
    public class KeyStringService<T> : BaseService<T>, IKeyStringService<T> where T: AKeyBase
    {
        protected readonly DbSet<T> _dbSet;

        public KeyStringService(
            ApplicationContext context,
            DbSet<T> dbSet
            ) : base(context)
        {
            _dbSet = dbSet;
        }

        public void AjouteSansSauver(T donnée)
        {
            _dbSet.Add(donnée);
        }

        public async Task<RetourDeService<T>> Ajoute(T donnée)
        {
            AjouteSansSauver(donnée);
            return await SaveChangesAsync(donnée);
        }

        public async Task<T> Lit(AKeyBase key)
        {
            return await _dbSet.Where(entité => entité.TexteKey == key.TexteKey).FirstOrDefaultAsync();
        }

        public async Task<List<T>> Liste(AKeyBase key)
        {
            return await _dbSet.Where(entité => key.EstSemblable(entité)).ToListAsync();
        }

        public async Task<List<T>> Liste()
        {
            return await _dbSet.ToListAsync();
        }

        protected Dictionary<string, FiltreurDeListe<T>> Filtreurs;
        protected Dictionary<string, TrieurDeListe<T>> Trieurs;

        private IQueryable<T> _ListeAOptions(IQueryable<T> iquery, OptionsDeListe options)
        {
            foreach (FiltreDeListe filtre in options.Filtres)
            {
                if (Filtreurs != null && Filtreurs.ContainsKey(filtre.Nom))
                {
                    FiltreurDeListe<T> filtreur = Filtreurs[filtre.Nom];
                    iquery = filtreur.AppliqueFiltre(iquery, filtre);
                }
            }
            foreach (TriDeListe tri in options.Tris)
            {
                if (Trieurs != null && Trieurs.ContainsKey(tri.Nom))
                {
                    TrieurDeListe<T> trieur = Trieurs[tri.Nom];
                    iquery = trieur.AppliqueTri(iquery, tri);
                }
            }
            return iquery;
        }

        public async Task<List<T>> ListeAOptions(OptionsDeListe options)
        {
            return await _ListeAOptions(_dbSet, options).ToListAsync();
        }

        public async Task<List<T>> ListeAOptions(AKeyBase key, OptionsDeListe options)
        {
            return await _ListeAOptions(_dbSet.Where(entité => key.EstSemblable(entité)), options).ToListAsync();
        }

        protected void _Edite(T donnée)
        {
            _context.Update(donnée);
        }

        public async Task<RetourDeService<T>> Edite(T donnée)
        {
            _Edite(donnée);
            return await SaveChangesAsync(donnée);
        }

        protected void _Supprime(T donnée)
        {
            _context.Remove(donnée);
        }

        public async Task<RetourDeService<T>> Supprime(T donnée)
        {
            _Supprime(donnée);
            return await SaveChangesAsync(donnée);
        }

    }
}
