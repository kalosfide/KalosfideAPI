using KalosfideAPI.Data;
using KalosfideAPI.Data.Constantes;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Enregistrement;
using KalosfideAPI.Erreurs;
using KalosfideAPI.Partages;
using KalosfideAPI.Partages.KeyParams;
using KalosfideAPI.Produits;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Sites
{
    class NbDeSite
    {
        public string Uid { get; set; }
        public int Rno { get; set; }
        public int Nb { get; set; }
    }

    class GèreEtat : Partages.KeyParams.GéreEtat<Site, SiteVue, EtatSite>
    {
        public GèreEtat(DbSet<EtatSite> dbSetEtat) : base(dbSetEtat)
        { }
        protected override EtatSite CréeEtatAjout(Site donnée)
        {
            EtatSite etat = new EtatSite
            {
                NomSite = donnée.NomSite,
                Titre = donnée.Titre,
                Etat = donnée.Etat,
                DateEtat = donnée.DateEtat,
                Date = DateTime.Now
            };
            return etat;
        }
        protected override EtatSite CréeEtatEdite(Site donnée, SiteVue vue)
        {
            bool modifié = false;
            EtatSite état = new EtatSite
            {
                Date = DateTime.Now
            };
            if (vue.NomSite != null && donnée.NomSite != vue.NomSite)
            {
                donnée.NomSite = vue.NomSite;
                état.NomSite = vue.NomSite;
                modifié = true;
            }
            if (vue.Titre != null && donnée.Titre != vue.Titre)
            {
                donnée.Titre = vue.Titre;
                état.Titre = vue.Titre;
                modifié = true;
            }
            if (vue.Etat != null && donnée.Etat != vue.Etat)
            {
                donnée.Etat = vue.Etat;
                état.Etat = vue.Etat;
                modifié = true;
            }
            if (vue.DateEtat != null && donnée.DateEtat != vue.DateEtat)
            {
                donnée.DateEtat = vue.DateEtat;
                état.DateEtat = vue.DateEtat;
                modifié = true;
            }
            return modifié ? état : null;
        }
    }

    public class SiteService : KeyUidRnoService<Site, SiteVue, EtatSite>, ISiteService
    {
        private IProduitService _produitService;

        public SiteService(ApplicationContext context, IProduitService produitService) : base(context)
        {
            _dbSet = _context.Site;
            _géreEtat = new GèreEtat(_context.EtatSite);
            _produitService = produitService;
            CréeVueAsync = CréeSiteVueAsync;
            dValideAjoute = ValideAjoute;
            dValideEdite = ValideEdite;
        }

        public Site CréeSite(Role role, EnregistrementFournisseurVue fournisseurVue)
        {
            Site site = new Site
            {
                Uid = role.Uid,
                Rno = role.Rno,
                NomSite = fournisseurVue.NomSite,
                Titre = fournisseurVue.Titre,
                Etat = TypeEtatSite.Nouveau
            };
            role.SiteUid = role.Uid;
            role.SiteRno = role.Rno;
            return site;
        }

        private async Task<SiteVue> CréeSiteVueAsync(Site site)
        {
            SiteVue vue = new SiteVue
            {
                Uid = site.Uid,
                Rno = site.Rno,
                NomSite = site.NomSite,
                Titre = site.Titre,
                NbProduits = await _produitService.NbDisponibles(site),
                Etat = site.Etat,
                DateEtat = site.DateEtat
            };
            return vue;
        }

        private SiteVue CréeVue(Site site, int nbProduits)
        {
            SiteVue vue = new SiteVue
            {
                Uid = site.Uid,
                Rno = site.Rno,
                NomSite = site.NomSite,
                Titre = site.Titre,
                NbProduits = nbProduits,
                Etat = site.Etat,
                DateEtat = site.DateEtat
            };
            return vue;
        }

        private SiteVue FixeNbProduits(SiteVue vue, int nb)
        {
            vue.NbProduits = nb;
            return vue;
        }

        protected override async Task<List<SiteVue>> CréeVues(ValideFiltre<Site> valideT, ValideFiltre<SiteVue> valideVue)
        {
            IQueryable<Site> sites = _context.Site;
            if (valideT != null)
            {
                sites = sites.Where(s => valideT(s));
            }
            var res1 = await sites.ToListAsync();

            IQueryable<NbDeSite> nbProduits = _context.Produit
                .GroupBy(p => new { p.Uid, p.Rno })
                .Select(eg => new NbDeSite{ Uid=eg.Key.Uid, Rno=eg.Key.Rno, Nb = eg.Count(p => eg.Key.Uid == p.Uid && eg.Key.Rno == p.Rno) });
            var res2 = await nbProduits.ToListAsync();
            nbProduits = _context.Produit
                .GroupBy(p => new { p.Uid, p.Rno })
                .Select(eg => new NbDeSite{ Uid=eg.Key.Uid, Rno=eg.Key.Rno, Nb = eg.Count(p => eg.Key.Uid == p.Uid && eg.Key.Rno == p.Rno && p.Prix > 0) });
            res2 = await nbProduits.ToListAsync();

            IQueryable<SiteVue> vues = sites.Join(nbProduits,
                site => new { site.Uid, site.Rno },
                nb => new { nb.Uid, nb.Rno },
                (site, nb) => CréeVue(site, nb.Nb));

            if (valideVue != null)
            {
                vues = vues.Where(v => valideVue(v));
            }
            List<SiteVue> liste = await vues.ToListAsync();
            return liste;
        }


        // implémentation des membres abstraits
        public override Task CopieVueDansDonnées(Site donnée, SiteVue vue)
        {
            donnée.NomSite = vue.NomSite;
            donnée.Titre = vue.Titre;
            return Task.CompletedTask;
        }

        public override Site NouvelleDonnée()
        {
            return new Site();
        }

        public override SiteVue CréeVue(Site donnée)
        {
            SiteVue vue = new SiteVue
            {
                NomSite = donnée.NomSite,
                Titre = donnée.Titre,
            };
            donnée.CopieKey(vue.KeyParam);
            return vue;
        }

        public override async Task<SiteVue> LitVue(KeyParam param)
        {
            Site site = await _context.Site.Where(e => e.EstSemblable(param)).FirstAsync();
            SiteVue vue = new SiteVue
            {
                NomSite = site.NomSite,
                Titre = site.Titre,
                Etat = site.Etat,
                DateEtat = site.DateEtat
            };
            site.CopieKey(vue.KeyParam);
            return vue;
        }
        // methodes
        public async Task<Site> LitSite(AKeyUidRno key)
        {
            return await _context.Site.Where(s => s.EstSemblable(key)).FirstAsync();
        }
        public async Task<EtatSite> Etat(AKeyUidRno key)
        {
            return await _context.EtatSite.Where(es => es.EstSemblable(key)).LastAsync();
        }
        public async Task<bool> Ouvert(AKeyUidRno key)
        {
            Site site = await LitSite(key);
            return site.Etat == TypeEtatSite.Actif && site.DateEtat < DateTime.Now; 
        }
        
        public async Task<RetourDeService> Ouvre(KeyUidRno key)
        {
            Site site = await LitSite(key);
            DateTime maintenant = DateTime.Now;
            switch (site.Etat)
            {
                case TypeEtatSite.Nouveau:
                    List<ProduitVue> produits = await _produitService.Disponibles(key.KeyParam);
                    if (produits.Count == 0)
                    {
                        // impossible d'ouvrir un site qui n'a pas de produits
                        return null;
                    }
                    break;
                case TypeEtatSite.Actif:
                    if (site.DateEtat < maintenant)
                    {
                        // le site est déjà ouvert
                        return new RetourDeService<Site>(TypeRetourDeService.Ok);
                    }
                    break;
                case TypeEtatSite.Inactif:
                    break;
                default:
                    // on ne fait rien
                    return new RetourDeService<Site>(TypeRetourDeService.Ok);
            }
            // mise à jour du site
            site.Etat = TypeEtatSite.Actif;
            site.DateEtat = maintenant;
            _context.Site.Update(site);
            // ajout de l'état
            EtatSite etat = new EtatSite
            {
                Uid = site.Uid,
                Rno = site.Rno,
                Etat = TypeEtatSite.Actif,
                DateEtat = maintenant,
                Date = maintenant
            };
            _context.EtatSite.Add(etat);
            return await SaveChangesAsync();
        }

        public async Task<RetourDeService> Ferme(KeyUidRno key, DateTime jusquA)
        {
            Site site = await LitSite(key);
            if (site.Etat != TypeEtatSite.Actif && site.Etat != TypeEtatSite.Inactif)
            {
                return new RetourDeService<Site>(TypeRetourDeService.Ok);
            }
            DateTime maintenant = DateTime.Now;
            string type;
            DateTime? date;
            if (jusquA < maintenant)
            {
                type = TypeEtatSite.Inactif;
                date = null;
            }
            else
            {
                type = TypeEtatSite.Actif;
                date = jusquA;
            }
            if (site.Etat != type || site.DateEtat != date)
            {
                site.Etat = type;
                site.DateEtat = date;
                _context.Site.Update(site);
                EtatSite etat = new EtatSite
                {
                    Uid = key.Uid,
                    Rno = key.Rno,
                    Etat = type,
                    DateEtat = date,
                    Date = maintenant
                };
                _context.EtatSite.Add(etat);
            }
            RetourDeService retour = await SaveChangesAsync();

            if (retour.Ok)
            {
                List<Commande> commandes = await _context.Commande
                    .Where(c => c.Date == null && c.LivraisonUid == key.Uid && c.LivraisonRno == key.Rno)
                    .ToListAsync();
                if (commandes.Count > 0)
                {
                    commandes.ForEach(c =>
                    {
                        c.Etat = TypeEtatCommande.Accepté;
                        _context.Commande.Update(c);
                    });
                    retour = await SaveChangesAsync();
                }
            }
            return retour;
        }

        public async Task<SiteVue> TrouveParNom(string nomSite)
        {
            Site site = await _context.Site.Where(s => s.NomSite == nomSite).FirstOrDefaultAsync();
            return site == null ? null : await CréeSiteVueAsync(site);
        }

        public async Task<bool> NomPris(string nomSite)
        {
            return await _dbSet.Where(site => site.NomSite == nomSite).AnyAsync();
        }

        public async Task<bool> NomPrisParAutre(AKeyUidRno key, string nomSite)
        {
            return await _dbSet.Where(site => site.NomSite == nomSite && (site.Uid != key.Uid || site.Rno != key.Rno)).AnyAsync();
        }

        public async Task<bool> TitrePris(string titre)
        {
            return await _dbSet.Where(site => site.Titre == titre).AnyAsync();
        }

        public async Task<bool> TitrePrisParAutre(AKeyUidRno key, string titre)
        {
            return await _dbSet.Where(site => site.Titre == titre && (site.Uid != key.Uid || site.Rno != key.Rno)).AnyAsync();
        }

        ErreurDeModel ErreurNomSite()
        {
            return new ErreurDeModel
            {
                Code = "nomSitePris",
                Description = "Il y a déjà un site avec ce nom."
            };
        }

        ErreurDeModel ErreurTitre()
        {
            return new ErreurDeModel
            {
                Code = "titrePris",
                Description = "Il y a déjà un site avec ce titre."
            };
        }

        ErreurDeModel ErreurKey()
        {
            return new ErreurDeModel
            {
                Code = "keySite",
                Description = "Il n'y a pas de site avec cette clé."
            };
        }

        private async Task ValideAjoute(Site donnée, ModelStateDictionary modelState)
        {
            if (await NomPris(donnée.NomSite))
            {
                ErreurNomSite().AjouteAModelState(modelState);
            }
            if (await TitrePris(donnée.Titre))
            {
                ErreurTitre().AjouteAModelState(modelState);
            }
        }

        private async Task ValideEdite(Site donnée, ModelStateDictionary modelState)
        {
            if (await NomPrisParAutre(donnée, donnée.NomSite))
            {
                modelState.TryAddModelError("nomSitePris", "Il y a déjà un site avec ce nom.");
            }
            if (await TitrePrisParAutre(donnée, donnée.Titre))
            {
                modelState.TryAddModelError("titrePris", "Il y a déjà un site avec ce titre.");
            }
        }
    }
}
