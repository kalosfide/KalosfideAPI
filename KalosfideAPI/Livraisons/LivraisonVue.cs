using KalosfideAPI.Data;
using System;
using System.Collections.Generic;

namespace KalosfideAPI.Livraisons
{
    public class LivraisonVue
    {
        public long Id { get; set; }

        public long FournisseurId { get; set; }
        public Role Fournisseur { get; set; }

        // le fournisseur verrouille la commande en fixant Livraison.Date
        public DateTime Date { get; set; }

        public ICollection<Commande> Commandes { get; set; }
    }
}
