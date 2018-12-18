using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Erreurs;
using KalosfideAPI.Partages.KeyParams;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Sites
{
    public class SiteService : KeyUidRnoService<Site, SiteVue>, ISiteService
    {
        public SiteService(ApplicationContext context) : base(context)
        {
            _dbSet = _context.Site;
        }
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
                Titre = donnée.Titre
            };
            FixeVueKey(donnée, vue);
            return vue;
        }

        public async Task<Site> TrouveParNom(string nomSite)
        {
            return await _dbSet.Where(site => site.NomSite == nomSite).FirstOrDefaultAsync();
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

        public override async Task ValideAjoute(Site donnée, ModelStateDictionary modelState)
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

        public override async Task ValideEdite(Site donnée, ModelStateDictionary modelState)
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
