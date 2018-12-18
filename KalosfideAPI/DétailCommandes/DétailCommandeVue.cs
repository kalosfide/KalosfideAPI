using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Produits;

namespace KalosfideAPI.DétailCommandes
{
    public class DétailCommandeVue : AKeyUidRnoNo2
    {
        // key
        public override string Uid { get; set; }
        public override int Rno { get; set; }
        public override long No { get; set; }

        public override string Uid2 { get; set; }
        public override int Rno2 { get; set; }
        public override long No2 { get; set; }

        // données
        public string TypeCommande { get; set; }
        public decimal Demande { get; set; }
        public decimal? Mesure { get; set; }

        public decimal? Prix { get; set; }

        // calculés
        public string NomClient { get; set; }
        public string NomCategorie { get; set; }
        public string NomProduit { get; set; }
        public string TypeMesure { get; set; }
        public decimal PrixUnitaire { get; set; }
    }
}
