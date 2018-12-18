using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Partages;
using KalosfideAPI.Partages.KeyParams;
using KalosfideAPI.Produits;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Commandes
{
    public class CommandeService : KeyUidRnoNoService<Commande, CommandeVue>, ICommandeService
    {
        private readonly IProduitService _produitService;

        public CommandeService(ApplicationContext context, IProduitService produitService) : base(context)
        {
            _dbSet = _context.Commande;
            _produitService = produitService;
        }

        public async Task<RetourDeService<Commande>> AjouteVue(CommandeVue vue)
        {
            vue.No = (await DernierNo(vue.Uid, vue.Rno)) + 1;
            Commande donnée = new Commande
            {
                Date = vue.Date,
            };
            FixeKey(donnée, vue);
            _context.Commande.Add(donnée);
            List<DétailCommande> details = vue.Lignes.Select(
                (CommandeVueDétail vd) =>
                {
                    return new DétailCommande
                    {
                        Uid = vue.Uid,
                        Rno = vue.Rno,
                        No = vue.No,
                        Uid2 = vd.Uid,
                        Rno2 = vd.Rno,
                        No2 = vd.No,
                        TypeCommande = vd.TypeCommande,
                        Demande = vd.Demande
                    };
                }).ToList();
            details.ForEach(d => _context.DétailCommande.Add(d));
            return await SaveChangesAsync(donnée);
        }

        public override Commande NouvelleDonnée()
        {
            return new Commande();
        }

        public override Task CopieVueDansDonnées(Commande donnée, CommandeVue vue)
        {
            donnée.LivraisonUid = vue.LivraisonUid;
            donnée.LivraisonRno = vue.LivraisonRno;
            donnée.LivraisonNo = vue.LivraisonNo;
            return Task.CompletedTask;
        }

        public IQueryable<Commande> Complète(IQueryable<Commande> commandes)
        {
            return commandes
                .Include(c => c.Client)
                .Include(c => c.DétailCommandes);
        }

        public override CommandeVue CréeVue(Commande donnée)
        {
            CommandeVue vue = new CommandeVue
            {
                Date = donnée.Date,
                LivraisonUid = donnée.LivraisonUid,
                LivraisonRno = donnée.LivraisonRno,
                LivraisonNo = donnée.LivraisonNo,
                NomClient = donnée.Client.Nom,
                Lignes = donnée.DétailCommandes.Select(d => new CommandeVueDétail
                {
                    Uid = d.Uid2,
                    Rno = d.Rno2,
                    No = d.No2,
                    TypeCommande = d.TypeCommande,
                    Demande = d.Demande,
                }).ToList(),
                Prix = donnée.Prix,
            };
            FixeVueKey(donnée, vue);
            return vue;
        }

        public async Task<Commande> CommandeOuverte(AKeyUidRno key)
        {
            Commande commande = await _context.Commande.Where(c => key.EstSemblable(c)).LastOrDefaultAsync();
            long no = commande == null ? 1 : commande.No + 1;
            if (commande == null || commande.Date != null)
            {
                commande = new Commande
                {
                    Uid = key.Uid,
                    Rno = key.Rno,
                    No = no
                };
                _context.Commande.Add(commande);
                await _context.SaveChangesAsync();
            }
            return commande;
        }

        public async Task<List<Commande>> ListeDuSite(AKeyUidRno key)
        {
            var site = await _context.Site.Where(s => s.Uid == key.Uid && s.Rno == key.Rno).FirstOrDefaultAsync();
            return await _context.Commande
                .Include(c => c.Client)
                .ThenInclude(cl => cl.Role)
                .Where(c => c.Client.Role.SiteUid == key.Uid && c.Client.Role.SiteRno == key.Rno)
                .ToListAsync();
        }

        public async Task<string> NomClient(Commande commande)
        {
            return await _context.Client
                .Where(client => client.Uid == commande.Uid && client.Rno == commande.Rno).Select(client => client.Nom)
                .FirstOrDefaultAsync();
        }

        public async Task<decimal?> Prix(Commande commande)
        {
            List<DétailCommande> détails = await _context.DétailCommande.Where(d => commande.EstSemblable(d.KeyParam)).ToListAsync();
                decimal? prix = -1;
                détails.ForEach(d =>
                {
                    if (d.Prix.HasValue)
                    {
                        prix += d.Prix.Value;
                    }
                });
                return prix;
        }
    }
}
