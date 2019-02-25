using KalosfideAPI.Data.Keys;
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
        public string Etat { get; set; }

        // calculés
        public string NomClient { get; set; }
        public bool? NouveauClient { get; set; }

        public int? NbDetails { get; set; }
        public List<CommandeVueDétail> Details { get; set; }

        public decimal? Prix { get; set; }

    }

    public class CommandeVueDétail
    {
        public long No { get; set; }

        public string TypeCommande { get; set; }
        public decimal Demande { get; set; }
        public decimal? AServir { get; set; }
        public decimal? Servis { get; set; }
    }
}
