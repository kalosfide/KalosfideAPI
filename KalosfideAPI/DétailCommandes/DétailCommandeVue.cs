using KalosfideAPI.Data;
using KalosfideAPI.Produits;

namespace KalosfideAPI.DétailCommandes
{
    public class DétailCommandeVue
    {
        public long ID { get; set; }

        public long CommandeId { get; set; }
        public Commande Commande { get; set; }

        public long ProduitID { get; set; }
        public ProduitVue Produit { get; set; }

        public int Quantité { get; set; }
    }
}
