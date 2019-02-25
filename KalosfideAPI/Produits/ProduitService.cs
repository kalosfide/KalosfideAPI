using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Erreurs;
using KalosfideAPI.Partages;
using KalosfideAPI.Partages.KeyParams;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Produits
{
    class GèreEtat : Partages.KeyParams.GéreEtat<Produit, ProduitVue, EtatProduit>
    {
        public GèreEtat(DbSet<EtatProduit> dbSetEtat) : base(dbSetEtat)
        { }
        protected override EtatProduit CréeEtatAjout(Produit produit)
        {
            EtatProduit etat = new EtatProduit
            {
                CatégorieNo = produit.CategorieNo,
                Nom = produit.Nom,
                TypeCommande = produit.TypeCommande,
                TypeMesure = produit.TypeMesure,
                Prix = produit.Prix,
                Date = DateTime.Now
            };
            return etat;
        }
        protected override EtatProduit CréeEtatEdite(Produit donnée, ProduitVue vue)
        {
            bool modifié = false;
            EtatProduit etat = new EtatProduit
            {
                Date = DateTime.Now
            };
            if (vue.CategorieNo != null && donnée.CategorieNo != vue.CategorieNo)
            {
                donnée.CategorieNo = vue.CategorieNo ?? 0;
                etat.CatégorieNo = vue.CategorieNo;
                modifié = true;
            }
            if (vue.Nom != null && donnée.Nom != vue.Nom)
            {
                donnée.Nom = vue.Nom;
                etat.Nom = vue.Nom;
                modifié = true;
            }
            if (vue.TypeCommande != null && donnée.TypeCommande != vue.TypeCommande)
            {
                donnée.TypeCommande = vue.TypeCommande;
                etat.TypeCommande = vue.TypeCommande;
                modifié = true;
            }
            if (vue.TypeMesure != null && donnée.TypeMesure != vue.TypeMesure)
            {
                donnée.TypeMesure = vue.TypeMesure;
                etat.TypeMesure = vue.TypeMesure;
                modifié = true;
            }
            if (vue.Prix != null && donnée.Prix != vue.Prix)
            {
                donnée.Prix = vue.Prix ?? 0;
                etat.Prix = vue.Prix;
                modifié = true;
            }
            return modifié ? etat : null;
        }
    }

    public class ProduitService : KeyUidRnoNoService<Produit, ProduitVue, EtatProduit>, IProduitService
    {
        public ProduitService(ApplicationContext context) : base(context)
        {
            _dbSet = _context.Produit;
            _inclutRelations = Complète;
            dValideAjoute = ValideAjoute;
            dValideEdite = ValideEdite;
            dValideSupprime = ValideSupprime;
        }

        public async Task<Produit> GetProduit(AKeyUidRnoNo key, DateTime date)
        {
            Produit produit = new Produit
            {
                Uid = key.Uid,
                Rno = key.Rno,
                No = key.No
            };
            IQueryable<EtatProduit> états = _context.EtatProduit.Where(ep => key.EstSemblable(ep) && ep.Date < date);
            produit.Nom = await états
                .Where(ep => ep.Nom != null)
                .Select(ep => ep.Nom)
                .LastAsync();
            produit.TypeCommande = await états
                .Where(ep => ep.TypeCommande != null)
                .Select(ep => ep.TypeCommande)
                .LastAsync();
            produit.TypeMesure = await états
                .Where(ep => ep.TypeMesure != null)
                .Select(ep => ep.TypeMesure)
                .LastAsync();
            produit.Prix = await états
                .Where(ep => ep.Prix != null)
                .Select(ep => ep.Prix?? 0)
                .LastAsync();

            return produit;
        }

        ErreurDeModel ErreurNomPris()
        {
            return new ErreurDeModel
            {
                Code = "nomPris",
                Description = "Vous avez déjà un produit avec ce nom."
            };
        }

        private async Task ValideAjoute(Produit donnée, ModelStateDictionary modelState)
        {
            if (await NomPris(donnée.Nom))
            {
                ErreurNomPris().AjouteAModelState(modelState);
            }
        }

        private async Task ValideEdite(Produit donnée, ModelStateDictionary modelState)
        {
            if (await NomPrisParAutre(donnée, donnée.Nom))
            {
                ErreurNomPris().AjouteAModelState(modelState);
            }
        }

        ErreurDeModel ErreurNonSupprimable()
        {
            return new ErreurDeModel
            {
                Code = "supprime",
                Description = "Des commandes ont été enregistrées pour ce produit."
            };
        }

        private async Task ValideSupprime(Produit donnée, ModelStateDictionary modelState)
        {
            bool avecCommandes = await _context.DétailCommande
                .Where(dc => donnée.Uid == dc.Uid2 && donnée.Rno == dc.Rno2 && donnée.No == dc.No2)
                .AnyAsync();
            if (avecCommandes)
            {
                ErreurNonSupprimable().AjouteAModelState(modelState);
            }
        }

        IQueryable<Produit> Complète(IQueryable<Produit> produits)
        {
            return produits.Include(p => p.Catégorie);
        }

        public async Task<string> NomCatégorie(Produit donnée)
        {
            return await _context.Catégorie
                .Where(catégorie => catégorie.Uid == donnée.Uid && catégorie.Rno == donnée.Rno && catégorie.No == donnée.CategorieNo)
                .Select(catégorie => catégorie.Nom)
                .FirstAsync();
        }

        public override Task CopieVueDansDonnées(Produit donnée, ProduitVue vue)
        {
            // inutilisé
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
                CategorieNo = donnée.CategorieNo,
                TypeCommande = donnée.TypeCommande,
                TypeMesure = donnée.TypeMesure,
                NomCategorie = donnée.Catégorie.Nom,
                Prix = donnée.Prix
            };
            vue.CopieKey(donnée.KeyParam);
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

        public async Task<List<ProduitVue>> Disponibles(KeyParam param)
        {
            return await Liste(param, (ProduitVue vue) => { return vue.Prix > 0; });
        }

        public async Task<int> NbDisponibles(AKeyUidRno keySite)
        {
            return await _dbSet.Where(p => p.Uid == keySite.Uid && p.Rno == keySite.Rno && p.Prix > 0).CountAsync();
        }
    }
}
