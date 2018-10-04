using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Partages.KeyString;

namespace KalosfideAPI.Produits
{
    public class ProduitVue: AKeyRIdNo
    {
        // identité
        public override string RoleId { get; set; }
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
