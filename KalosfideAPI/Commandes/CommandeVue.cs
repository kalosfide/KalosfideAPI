using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.DétailCommandes;
using KalosfideAPI.Livraisons;
using KalosfideAPI.Produits;
using System;
using System.Collections.Generic;

namespace KalosfideAPI.Commandes
{
    public class CommandeVue : AKeyUidRnoNo
    {
        // identité
        public override string Uid { get; set; }
        public override int Rno { get; set; }
        public override long No { get; set; }

        // données
        public DateTime? Date { get; set; }

        public string LivraisonUid { get; set; }
        public int? LivraisonRno { get; set; }
        public long? LivraisonNo { get; set; }

        // calculés
        public string NomClient { get; set; }
        public List<CommandeVueDétail> Lignes { get; set; }
        public decimal? Prix { get; set; }
    }

    public class CommandeVueDétail : AKeyUidRnoNo
    {
        public override string Uid { get; set; }
        public override int Rno { get; set; }
        public override long No { get; set; }

        public string TypeCommande { get; set; }
        public decimal Demande { get; set; }
    }
}
