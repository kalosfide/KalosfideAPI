using KalosfideAPI.Commandes;
using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Partages;
using KalosfideAPI.Partages.KeyParams;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Livraisons
{
    public class LivraisonService : KeyUidRnoNoService<Livraison, LivraisonVue, KeyUidRnoNo>, ILivraisonService
    {
        private readonly ICommandeService _commandeService;

        public LivraisonService(ApplicationContext context, ICommandeService commandeService) : base(context)
        {
            _commandeService = commandeService;
        }

        /// <summary>
        /// appelé à la création du Fournisseur
        /// </summary>
        /// <param name="keyFournisseur"></param>
        /// <returns></returns>
        public void CréePremièreLivraison(AKeyUidRno keyFournisseur)
        {
            Livraison livraison = new Livraison
            {
                Uid = keyFournisseur.Uid,
                Rno = keyFournisseur.Rno,
                No = 1,
            };
            _context.Livraison.Add(livraison);
        }

        /// <summary>
        /// retourne la vue de la dernière Livraison d'un Fournisseur
        /// </summary>
        /// <param name="keyFournisseur">key du client</param>
        /// <returns></returns>
        public async Task<LivraisonVue> LivraisonEnCours(AKeyUidRno keyFournisseur)
        {
            Livraison livraison = await _context.Livraison
                .Where(l => l.Uid == keyFournisseur.Uid && l.Rno == keyFournisseur.Rno)
                .LastAsync();
            LivraisonVue vue = new LivraisonVue
            {
                Commandes = await _commandeService.CommandesDeLivraison(keyFournisseur)
            };
            vue.CopieKey(livraison.KeyParam);
            return vue;
        }


        public override Task CopieVueDansDonnées(Livraison donnée, LivraisonVue vue)
        {
            throw new NotImplementedException();
        }

        public override LivraisonVue CréeVue(Livraison donnée)
        {
            throw new NotImplementedException();
        }

        public override Livraison NouvelleDonnée()
        {
            return new Livraison();
        }

        public async Task<Livraison> Dernière(AKeyUidRno keyFournisseur)
        {
            Livraison livraison = await _context.Livraison.Where(l => l.EstSemblable(keyFournisseur)).LastOrDefaultAsync();
            return livraison;
        }
    }
}
