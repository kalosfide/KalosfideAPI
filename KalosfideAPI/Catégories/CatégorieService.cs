using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Erreurs;
using KalosfideAPI.Partages.KeyParams;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Catégories
{
    public class CatégorieService : KeyUidRnoNoService<Catégorie, CatégorieVue>, ICatégorieService
    {
        public CatégorieService(ApplicationContext context) : base(context)
        {
            _dbSet = _context.Catégorie;
            _inclutRelations = Complète;
        }

        public override Task CopieVueDansDonnées(Catégorie donnée, CatégorieVue vue)
        {
            donnée.Nom = vue.Nom;
            return Task.CompletedTask;
        }

        public override Catégorie NouvelleDonnée()
        {
            return new Catégorie();
        }

        IQueryable<Catégorie> Complète(IQueryable<Catégorie> données)
        {
            return données.Include(d => d.Produits);
        }

        public override CatégorieVue CréeVue(Catégorie donnée)
        {
            CatégorieVue vue = new CatégorieVue
            {
                Nom = donnée.Nom,
                NbProduits = donnée.Produits.Count
            };
            FixeVueKey(donnée, vue);
            return vue;
        }

        public async Task<bool> NomPris(string nom)
        {
            return await _dbSet.Where(Catégorie => Catégorie.Nom == nom).AnyAsync();
        }

        public async Task<bool> NomPrisParAutre(AKeyUidRnoNo key, string nom)
        {
            return await _dbSet.Where(Catégorie => Catégorie.Nom == nom && (Catégorie.Uid != key.Uid || Catégorie.Rno != key.Rno || Catégorie.No != key.No)).AnyAsync();
        }

        ErreurDeModel ErreurNomPris()
        {
            return new ErreurDeModel
            {
                Code = "nomPris",
                Description = "Il y a déjà une catégorie avec ce nom."
            };
        }

        public override async Task ValideAjoute(Catégorie donnée, ModelStateDictionary modelState)
        {
            if (await NomPris(donnée.Nom))
            {
                ErreurNomPris().AjouteAModelState(modelState);
            }
        }

        public override async Task ValideEdite(Catégorie donnée, ModelStateDictionary modelState)
        {
            if (await NomPrisParAutre(donnée, donnée.Nom))
            {
                ErreurNomPris().AjouteAModelState(modelState);
            }
        }

        protected override void EditeSansSauver(Catégorie donnée)
        {
            base.EditeSansSauver(donnée);
        }
    }
}
