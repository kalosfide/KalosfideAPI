using KalosfideAPI.Data;
using KalosfideAPI.Data.Constantes;
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
    public class CommandeService : KeyUidRnoNoService<Commande, CommandeVue, KeyUidRnoNo>, ICommandeService
    {
        private readonly IProduitService _produitService;

        public CommandeService(ApplicationContext context, IProduitService prroduitService) : base(context)
        {
            _dbSet = _context.Commande;
            _produitService = prroduitService;
        }

        /// <summary>
        /// appelé à la création du Client
        /// </summary>
        /// <param name="keyClient"></param>
        /// <param name="keySite"></param>
        /// <returns></returns>
        public void CréePremièreCommande(AKeyUidRno keyClient, AKeyUidRno keySite)
        {
            Commande commande = new Commande
            {
                Uid = keyClient.Uid,
                Rno = keyClient.Rno,
                No = 1,
                LivraisonUid = keySite.Uid,
                LivraisonRno = keySite.Rno,
                Etat = TypeEtatCommande.Nouveau
            };
            _context.Commande.Add(commande);
        }

        /// <summary>
        /// crée une vue avec Date et Details sans LivraisonNo, NomClient, NouveauClient, Prix, NbDetails
        /// avec des détails sans AServir, Servis
        /// </summary>
        /// <param name="commande">la commande à transformer</param>
        /// <returns></returns>
        private CommandeVue CréeVueCommander(Commande commande)
        {
            CommandeVue vue = new CommandeVue
            {
                LivraisonUid = commande.LivraisonUid,
                LivraisonRno = commande.LivraisonRno,
                LivraisonNo = commande.LivraisonNo,
                Date = commande.Date,
                Details = commande.DétailCommandes.Select(d => new CommandeVueDétail
                {
                    No = d.No2,
                    TypeCommande = d.TypeCommande,
                    Demande = d.Demande,
                }).ToList(),
            };
            vue.CopieKey(commande.KeyParam);
            return vue;
        }

        /// <summary>
        /// retourne la vue à éditer de la dernière Commande d'un client
        /// </summary>
        /// <param name="keyClient"></param>
        /// <returns></returns>
        public async Task<CommandeVue> CommandeEnCours(AKeyUidRno keyClient)
        {
            Commande commande = await _context.Commande
                .Where(c => c.Uid == keyClient.Uid && c.Rno == keyClient.Rno)
                .Include(c => c.DétailCommandes)
                .LastAsync();
            CommandeVue vue = CréeVueCommander(commande);
            return vue;
        }

        /// <summary>
        ///  crée la commande d'un client ou fixe sa date
        ///  met à jour les détails de la commande pour qu'ils correspondent à ceux de la vue
        /// </summary>
        /// <param name="vue"></param>
        /// <returns></returns>
        public async Task<RetourDeService> EnvoiBon(CommandeVue vue)
        {
            List<CommandeVueDétail> ajouter = new List<CommandeVueDétail>(vue.Details);
            Commande commande = await _context.Commande
                .Where(c => vue.EstSemblable(c))
                .LastOrDefaultAsync();
            commande.Date = vue.Date;
            _context.Commande.Update(commande);
            DétailCommande[] actuels = await _context.DétailCommande.Where(dc => vue.EstSemblable(dc)).ToArrayAsync();
            List<DétailCommande> supprimer = new List<DétailCommande>();
            List<DétailCommande> editer = new List<DétailCommande>();
            foreach (DétailCommande détail in actuels)
            {
                CommandeVueDétail vdétail = vue.Details.Find(d => d.No == détail.No2);
                if (vdétail == null)
                {
                    supprimer.Add(détail);
                }
                else
                {
                    if (vdétail.TypeCommande != détail.TypeCommande || vdétail.Demande != détail.Demande)
                    {
                        détail.TypeCommande = vdétail.TypeCommande;
                        détail.Demande = vdétail.Demande;
                        editer.Add(détail);
                    }
                    ajouter.Remove(vdétail);
                }
            }
            _context.DétailCommande.RemoveRange(supprimer);
            _context.DétailCommande.UpdateRange(editer);
            _context.DétailCommande.AddRange(ajouter
                .Select(vd => new DétailCommande
                {
                    Uid = vue.Uid,
                    Rno = vue.Rno,
                    No = vue.No,
                    Uid2 = commande.LivraisonUid,
                    Rno2 = commande.LivraisonRno,
                    No2 = vd.No,
                    TypeCommande = vd.TypeCommande,
                    Demande = vd.Demande
                }));
            return await SaveChangesAsync();
        }

        /// <summary>
        /// crée une vue sans la clé de Livraison avec Date, NomClient, NouveauClient si vrai, Etat
        /// avec Details avec No (du produit), TypeCommande (si nécessaire), Demande, AServir
        /// 
        /// </summary>
        /// <param name="commande">la commande à transformer</param>
        /// <returns></returns>
        public CommandeVue CréeVueDeLivraison(Commande commande)
        {
            CommandeVue vue = new CommandeVue
            {
                Date = commande.Date,
                NomClient = commande.Client.Nom,
                Etat = commande.Etat,
                Details = commande.DétailCommandes.Select(d =>
                {
                    CommandeVueDétail détail = new CommandeVueDétail
                    {
                        No = d.No2,
                        TypeCommande = d.TypeCommande,
                        Demande = d.Demande,
                        AServir = d.AServir
                    };
                    return détail;
                }).ToList(),
            };
            if (commande.Client.Role.Etat == TypeEtatRole.Nouveau)
            {
                vue.NouveauClient = true;
            }
            vue.CopieKey(commande.KeyParam);
            return vue;
        }

        /// <summary>
        /// retourne la liste des commandes d'un fournisseur d'état Accepté et s'il n'y en a pas d'état Nouveau
        /// </summary>
        /// <param name="keyFournisseur"></param>
        /// <returns></returns>
        public async Task<List<CommandeVue>> CommandesDeLivraison(AKeyUidRno keyFournisseur)
        {
            List<Commande> commandes = await CommandesParEtat(keyFournisseur, TypeEtatCommande.Accepté);
            if (commandes.Count == 0)
            {
                commandes = await CommandesParEtat(keyFournisseur, TypeEtatCommande.Nouveau);
            }
            return commandes
                .Select(c => CréeVueAccepter(c))
                .ToList();
        }

        /// <summary>
        /// crée une vue avec Date, NomClient, NouveauClient, NbDetails sans Details, LivraisonNo, Prix
        /// </summary>
        /// <param name="commande">la commande à transformer</param>
        /// <returns></returns>
        public CommandeVue CréeVueAccepter(Commande commande)
        {
            CommandeVue vue = new CommandeVue
            {
                LivraisonUid = commande.LivraisonUid,
                LivraisonRno = commande.LivraisonRno,
                Date = commande.Date,
                NomClient = commande.Client.Nom,
                NouveauClient = commande.Client.Role.Etat == TypeEtatRole.Nouveau,
                NbDetails = commande.DétailCommandes.Count
            };
            vue.CopieKey(commande.KeyParam);
            return vue;
        }

        /// <summary>
        /// retourne la liste des commandes d'un fournisseur d'état etatCommande
        /// </summary>
        /// <param name="keyFournisseur"></param>
        /// <param name="etatCommande">TypeEtatCommande</param>
        /// <returns></returns>
        private async Task<List<Commande>> CommandesParEtat(AKeyUidRno keyFournisseur, string etatCommande)
        {
            List<Commande> commandes = await _context.Commande
                .Where(c => c.LivraisonUid == keyFournisseur.Uid && c.LivraisonRno == keyFournisseur.Rno && c.Etat == etatCommande)
                .ToListAsync();
            return commandes
                .Where(c => c.Client.Role.Etat == TypeEtatRole.Actif || c.Client.Role.Etat == TypeEtatRole.Nouveau)
                .Where(c => c.DétailCommandes.Count > 0)
                .ToList();
        }

        /// <summary>
        /// retourne le nombre des commandes d'un fournisseur d'état etatCommande
        /// </summary>
        /// <param name="keyFournisseur"></param>
        /// <returns></returns>
        public async Task<int> NbCommandesParEtat(AKeyUidRno keyFournisseur, string etatCommande)
        {
            return (await CommandesParEtat(keyFournisseur, etatCommande)).Count();
        }

        /// <summary>
        /// retourne le nombre des commandes d'un fournisseur d'état Nouveau
        /// </summary>
        /// <param name="keyFournisseur"></param>
        /// <returns></returns>
        public async Task<int> NbOuvertes(AKeyUidRno keyFournisseur)
        {
            return await NbCommandesParEtat(keyFournisseur, TypeEtatCommande.Nouveau);
        }

        /// <summary>
        /// retourne la liste des vues commandes d'un fournisseur d'état Nouveau
        /// </summary>
        /// <param name="keyFournisseur"></param>
        /// <returns></returns>
        public async Task<List<CommandeVue>> VuesOuvertes(AKeyUidRno keyFournisseur)
        {
            return (await CommandesParEtat(keyFournisseur, TypeEtatCommande.Nouveau))
                .Select(c => CréeVueAccepter(c))
                .ToList();
        }

        /// <summary>
        /// return null si les vues à enregistrer ne correspondent pas aux commandes ouvertes
        /// sinon fixe l'état des commandes ouvertes sur accepté ou refusé suivant les vues et retourne les commandes
        /// </summary>
        /// <param name="keyFournisseur"></param>
        /// <param name="vues"></param>
        /// <returns></returns>
        public async Task<List<Commande>> VérifieEnregistre(AKeyUidRno keyFournisseur, List<CommandeVue> vues)
        {
            List<Commande> commandes = await CommandesParEtat(keyFournisseur, TypeEtatCommande.Nouveau);
            if (commandes.Count == 0)
            {
                return null;
            }
            var àVérifier = commandes.Join(vues, c => c.Uid, v => v.Uid, (c, v) => new { c, v }).ToList();
            if (àVérifier.Count != commandes.Count || àVérifier.Count != vues.Count)
            {
                return null;
            }
            àVérifier.ForEach(a =>
            {
                a.c.Etat = a.v.Etat;
            });
            return commandes;
        }

        /// <summary>
        /// s'il y a des commandes acceptées, crée une livraison et fixe leur LivraisonNo
        /// enregistre les changements dans la base de données
        /// </summary>
        /// <param name="keyFournisseur"></param>
        /// <param name="vérifiées">les commandes ouvertes qui sont passées par VérifieEnregistre</param>
        /// <returns></returns>
        public async Task<RetourDeService> Enregistre(AKeyUidRno keyFournisseur, List<Commande> vérifiées)
        {
            List<Commande> acceptées = vérifiées.Where(c => c.Etat == TypeEtatCommande.Accepté).ToList();

            RetourDeService retour;
            if (acceptées.Count > 0)
            {
                Livraison dernièreLivraison = await _context.Livraison
                    .Where(l => l.Uid == keyFournisseur.Uid && l.Rno == keyFournisseur.Rno)
                    .LastOrDefaultAsync();
                retour = await SaveChangesAsync();
                if (!retour.Ok)
                {
                    return retour;
                }
                acceptées.ForEach(c => { c.LivraisonNo = dernièreLivraison.No; });
            }
            vérifiées.ForEach(c =>
            {
                _context.Commande.Update(c);
                Commande c1 = new Commande
                {
                    Uid = c.Uid,
                    Rno = c.Rno,
                    No = 1,
                    LivraisonUid = c.LivraisonUid,
                    LivraisonRno = c.LivraisonRno,
                    Etat = TypeEtatCommande.Nouveau,
                };
                _context.Commande.Add(c1);
            });
            return await SaveChangesAsync();
        }

        /// <summary>
        /// crée une vue avec Date, NomClient, NouveauClient, NbDetails sans Details, LivraisonNo, Prix
        /// </summary>
        /// <param name="commande">la commande à transformer</param>
        /// <returns></returns>
        public CommandeVue CréeVueAPréparer(Commande commande)
        {
            CommandeVue vue = new CommandeVue
            {
                Date = commande.Date,
                NomClient = commande.Client.Nom,
                Details = commande.DétailCommandes.Select(d => new CommandeVueDétail
                {
                    No = d.No2,
                    TypeCommande = d.TypeCommande,
                    Demande = d.Demande,
                    AServir = d.AServir
                }).ToList(),
            };
            vue.CopieKey(commande.KeyParam);
            return vue;
        }

        /// <summary>
        /// retourne le nombre des commandes d'un fournisseur d'état Accepté
        /// </summary>
        /// <param name="keyFournisseur"></param>
        /// <returns></returns>
        public async Task<int> NbApréparer(AKeyUidRno keyFournisseur)
        {
            return await NbCommandesParEtat(keyFournisseur, TypeEtatCommande.Accepté);
        }

        /// <summary>
        /// retourne la liste des vues des commandes d'un fournisseur d'état Accepté
        /// </summary>
        /// <param name="keyFournisseur"></param>
        /// <returns></returns>
        public async Task<List<CommandeVue>> VuesAPréparer(AKeyUidRno keyFournisseur)
        {
            return (await CommandesParEtat(keyFournisseur, TypeEtatCommande.Accepté))
                .Select(c => CréeVueAPréparer(c))
                .ToList();
        }



        // inutile?
        public async Task<RetourDeService> FermeOuvertes(AKeyUidRno keyFournisseur)
        {
            DateTime date = DateTime.Now;
            List<Commande> fermer = await _context.Commande
                .Where(c => c.LivraisonUid == keyFournisseur.Uid && c.LivraisonRno == keyFournisseur.Rno && c.Date == null)
                .Include(c => c.DétailCommandes)
                .Where(c => c.DétailCommandes.Count > 0)
                .ToListAsync();
            fermer.ForEach(c => c.Date = date);
            _context.Commande.UpdateRange(fermer);
            return await SaveChangesAsync();
        }


        private CommandeVue CréeVueNouvelle(Commande donnée)
        {
            CommandeVue vue = new CommandeVue
            {
                Date = donnée.Date,
                NomClient = donnée.Client.Nom,
                NbDetails = donnée.DétailCommandes.Count
            };
            vue.CopieKey(donnée.KeyParam);
            return vue;
        }

        public async Task<List<CommandeVue>> NouvellesCommandes(AKeyUidRno key)
        {
            return await _context.Commande
                .Where(c => c.LivraisonUid == key.Uid && c.LivraisonRno == key.Rno && c.LivraisonNo == null)
                .Include(c => c.Client)
                .Include(c => c.DétailCommandes)
                .Select(c => CréeVueNouvelle(c))
                .ToListAsync();
        }

        public override Commande NouvelleDonnée()
        {
            return new Commande();
        }

        public override Task CopieVueDansDonnées(Commande donnée, CommandeVue vue)
        {
            donnée.LivraisonUid = vue.LivraisonUid;
            donnée.LivraisonRno = vue.LivraisonRno?? 0;
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
                Details = donnée.DétailCommandes.Select(d => new CommandeVueDétail
                {
                    No = d.No2,
                    TypeCommande = d.TypeCommande,
                    Demande = d.Demande,
                }).ToList(),
                Prix = donnée.Prix,
            };
            vue.CopieKey(donnée.KeyParam);
            return vue;
        }

        /// <summary>
        /// retourne la Commande ouverte (sans date) d'un client
        /// crée une nouvelle Commande s'il n'y en a pas d'ouvertes
        /// </summary>
        /// <param name="keyClient"></param>
        /// <returns></returns>
        public async Task<Commande> Ouverte(AKeyUidRno keyClient)
        {
            Commande commande = await _context.Commande
                .Where(c => c.Uid == keyClient.Uid && c.Rno == keyClient.Rno)
                .LastOrDefaultAsync();
            long no = 1;
            if (commande != null)
            {
                if (commande.Date != null)
                {
                    no = commande.No + 1;
                }
                commande = null;
            }
            if (commande == null)
            {
                commande = new Commande
                {
                    Uid = keyClient.Uid,
                    Rno = keyClient.Rno,
                    No = no,
                };
                _context.Commande.Add(commande);
                RetourDeService retour = await SaveChangesAsync(commande);
                if (!retour.Ok)
                {
                    return null;
                }
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
