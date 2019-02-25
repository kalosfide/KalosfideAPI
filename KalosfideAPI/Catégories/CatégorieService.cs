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
    class GèreEtat : Partages.KeyParams.GéreEtat<Catégorie, CatégorieVue, EtatCatégorie>
    {
        public GèreEtat(DbSet<EtatCatégorie> dbSetEtat) : base(dbSetEtat)
        { }
        protected override EtatCatégorie CréeEtatAjout(Catégorie donnée)
        {
            EtatCatégorie etat = new EtatCatégorie
            {
                Nom = donnée.Nom,
                Date = DateTime.Now
            };
            return etat;
        }
        protected override EtatCatégorie CréeEtatEdite(Catégorie donnée, CatégorieVue vue)
        {
            bool modifié = false;
            EtatCatégorie état = new EtatCatégorie
            {
                Date = DateTime.Now
            };
            if (vue.Nom != null && donnée.Nom != vue.Nom)
            {
                donnée.Nom = vue.Nom;
                état.Nom = vue.Nom;
                modifié = true;
            }
            return modifié ? état : null;
        }
    }
    public class CatégorieService : KeyUidRnoNoService<Catégorie, CatégorieVue, EtatCatégorie>, ICatégorieService
    {
        public CatégorieService(ApplicationContext context) : base(context)
        {
            _dbSet = _context.Catégorie;
            _géreEtat = new GèreEtat(_context.EtatCatégorie);
            _inclutRelations = Complète;
            dValideAjoute = ValideAjoute;
            dValideEdite = ValideEdite;
            dValideSupprime = ValideSupprime;
        }

        public async Task<Catégorie> GetCatégorie(AKeyUidRnoNo key, DateTime date)
        {
            Catégorie catégorie = new Catégorie
            {
                Uid = key.Uid,
                Rno = key.Rno,
                No = key.No
            };
            IQueryable<EtatCatégorie> états = _context.EtatCatégorie.Where(ec => key.EstSemblable(ec) && ec.Date < date);
            catégorie.Nom = await états
                .Where(ec => ec.Nom != null)
                .Select(ec => ec.Nom)
                .LastAsync();
            return catégorie;
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

        private async Task ValideAjoute(Catégorie donnée, ModelStateDictionary modelState)
        {
            if (await NomPris(donnée.Nom))
            {
                ErreurNomPris().AjouteAModelState(modelState);
            }
        }

        private async Task ValideEdite(Catégorie donnée, ModelStateDictionary modelState)
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
                Description = "Des produits ont été enregistrées dans cette catégorie."
            };
        }

        private async Task ValideSupprime(Catégorie donnée, ModelStateDictionary modelState)
        {
            bool avecProduits = await _context.Produit
                .Where(p => donnée.Uid == p.Uid && donnée.Rno == p.Rno && donnée.No == p.No)
                .AnyAsync();
            if (avecProduits)
            {
                ErreurNonSupprimable().AjouteAModelState(modelState);
            }
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
            vue.CopieKey(donnée.KeyParam);
            return vue;
        }
    }
}
