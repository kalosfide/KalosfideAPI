using KalosfideAPI.Data;

namespace KalosfideAPI.Produits
{
    public class ProduitVue
    {
        public long Id { get; set; }

        public long FournisseurId { get; set; }
        public Role Fournisseur { get; set; }

        public string Nom { get; set; }
        public string Description { get; set; }
        public bool Indisponible { get; set; }
    }
}
