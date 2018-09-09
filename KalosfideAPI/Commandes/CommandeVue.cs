using KalosfideAPI.Data;
using KalosfideAPI.DétailCommandes;
using KalosfideAPI.Livraisons;
using System;
using System.Collections.Generic;

namespace KalosfideAPI.Commandes
{
    public class CommandeVue
    {
        public int No { get; set; }

        public string ClientUtilisateurId { get; set; }
        public int ClientRoleNo { get; set; }
        public Role Client { get; set; }

        public string FournisseurUtilisateurId { get; set; }
        public int FournisseurRoleNo { get; set; }
        public long LivraisonID { get; set; }
        public LivraisonVue Livraison { get; set; }

        public DateTime Date { get; set; }

        public ICollection<DétailCommandeVue> Détails { get; set; }
    }
}
