using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Partages.KeyParams;
using KalosfideAPI.Produits;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.DétailCommandes
{
    public class DétailCommandeService : KeyUidRnoNo2Service<DétailCommande, DétailCommandeVue, KeyUidRnoNo2>, IDétailCommandeService
    {
        private readonly IProduitService _produitService;

        public DétailCommandeService(ApplicationContext context, IProduitService produitService) : base(context)
        {
            _dbSet = _context.DétailCommande;
            _produitService = produitService;
        }

        IQueryable<DétailCommande> Complète(IQueryable<DétailCommande> données)
        {
            return données
                .Include(d => d.Commande)
                    .ThenInclude(c => c.Client)
                .Include(d => d.Produit);
        }

        public override DétailCommande NouvelleDonnée()
        {
            return new DétailCommande();
        }

        public override Task CopieVueDansDonnées(DétailCommande donnée, DétailCommandeVue vue)
        {
            donnée.TypeCommande = vue.TypeCommande;
            donnée.Demande = vue.Demande;
            donnée.AServir = vue.Mesure;
            donnée.Prix = vue.Prix;
            return Task.CompletedTask;
        }

        public override DétailCommandeVue CréeVue(DétailCommande donnée)
        {
            DétailCommandeVue vue = new DétailCommandeVue
            {
                NomClient = donnée.Commande.Client.Nom,
                NomProduit = donnée.Produit.Nom,
                PrixUnitaire = donnée.Produit.Prix,
                TypeCommande = donnée.TypeCommande,
                Demande = donnée.Demande,
                Mesure = donnée.AServir,
                Prix = donnée.Prix
            };
            vue.CopieKey(donnée.KeyParam);
            return vue;
        }

        public Task<DétailCommande> Lit(KeyUidRnoNo2 param)
        {
            return _context.DétailCommande
                .Where(d => d.EstSemblable(param))
                .FirstOrDefaultAsync();
        }

        IQueryable<DétailCommande> QueryClient(KeyUidRno key)
        {
            return _context.DétailCommande.Where(d => key.EstSemblable(d));
        }

        IQueryable<DétailCommande> QuerySite(KeyUidRno key)
        {
            return _context.DétailCommande.Where(d => key.EstSemblable(Key2(d)));
        }

        public async Task<List<DétailCommande>> ListeClient(KeyUidRno key)
        {
            return await _context.DétailCommande.Where(d => key.EstSemblable(d)).ToListAsync();
        }

        public async Task<List<DétailCommande>> ListeSite(KeyUidRno key)
        {
            return await _context.DétailCommande.Where(d => key.EstSemblable(Key2(d))).ToListAsync();
        }
    }
}
