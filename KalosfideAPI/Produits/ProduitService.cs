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

namespace KalosfideAPI.Produits
{
    public class ProduitService : KeyUidRnoNoService<Produit, ProduitVue>, IProduitService
    {
        public ProduitService(ApplicationContext context) : base(context)
        {
            _dbSet = _context.Produit;
            _inclutRelations = Complète;
        }

        IQueryable<Produit> Complète(IQueryable<Produit> produits)
        {
            return produits.Include(p => p.EtatPrix).Include(p => p.Catégorie);
        }

        public async Task<string> NomCatégorie(Produit donnée)
        {
            return await _context.Catégorie
                .Where(catégorie => catégorie.Uid == donnée.Uid && catégorie.Rno == donnée.Rno && catégorie.No == donnée.CatégorieNo)
                .Select(catégorie => catégorie.Nom)
                .FirstAsync();
        }

        public decimal Prix(Produit donnée, DateTime date)
        {
            EtatPrix etat = donnée.EtatPrix.Where(e => e.Date <= date).LastOrDefault();
            return etat != null ? etat.Prix : 0;
        }

        public decimal Prix(Produit donnée)
        {
            return Prix(donnée, DateTime.Now);
        }

        public override Task CopieVueDansDonnées(Produit donnée, ProduitVue vue)
        {
            donnée.Nom = vue.Nom;
            donnée.CatégorieNo = vue.CategorieNo;
            donnée.TypeCommande = vue.TypeCommande;
            donnée.TypeMesure = vue.TypeMesure;
            return Task.CompletedTask;
        }

        public override Produit NouvelleDonnée()
        {
            return new Produit();
        }

        public override ProduitVue CréeVue(Produit donnée)
        {
            ProduitVue vue = new ProduitVue
            {
                Nom = donnée.Nom,
                CategorieNo = donnée.CatégorieNo,
                TypeCommande = donnée.TypeCommande,
                TypeMesure = donnée.TypeMesure,
                NomCategorie = donnée.Catégorie.Nom,
                Prix = Prix(donnée)
            };
            FixeVueKey(donnée, vue);
            return vue;
        }

        public async Task<bool> NomPris(string nom)
        {
            return await _dbSet.Where(produit => produit.Nom == nom).AnyAsync();
        }

        public async Task<bool> NomPrisParAutre(AKeyUidRnoNo key, string nom)
        {
            return await _dbSet.Where(produit => produit.Nom == nom && (produit.Uid != key.Uid || produit.Rno != key.Rno || produit.No != key.No)).AnyAsync();
        }

        ErreurDeModel ErreurNomPris()
        {
            return new ErreurDeModel
            {
                Code = "nomPris",
                Description = "Vous avez déjà un produit avec ce nom."
            };
        }

        public override async Task ValideAjoute(Produit donnée, ModelStateDictionary modelState)
        {
            if (await NomPris(donnée.Nom))
            {
                ErreurNomPris().AjouteAModelState(modelState);
            }
        }

        public override async Task ValideEdite(Produit donnée, ModelStateDictionary modelState)
        {
            if (await NomPrisParAutre(donnée, donnée.Nom))
            {
                ErreurNomPris().AjouteAModelState(modelState);
            }
        }

        protected override void EditeSansSauver(Produit donnée)
        {
            base.EditeSansSauver(donnée);
        }

        public async Task<List<ProduitVue>> Disponibles(KeyParam param)
        {
            return await Liste(param, (ProduitVue vue) => { return vue.Prix >= 0; });
        }
    }
}
