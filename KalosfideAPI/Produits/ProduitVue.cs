using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Partages.KeyParams;

namespace KalosfideAPI.Produits
{
    public class ProduitVue: AKeyUidRnoNo
    {
        // identité
        public override string Uid { get; set; }
        public override int Rno { get; set; }
        public override long No { get; set; }

        // données
        public string Nom { get; set; }
        public string Description { get; set; }

        public decimal Unité { get; set; }
        public bool QuantitéVautUnités { get; set; }

        // calculés
        public decimal PrixUnitaire { get; set; }
        public bool Indisponible { get; set; }
    }
}
