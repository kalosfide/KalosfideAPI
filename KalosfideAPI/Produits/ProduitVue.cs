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
        public long? CategorieNo { get; set; }

        public string TypeCommande { get; set; }
        public string TypeMesure { get; set; }
        public decimal? Prix { get; set; }

        // calculés
        public string NomCategorie { get; set; }
    }
}
