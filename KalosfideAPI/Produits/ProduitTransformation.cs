using System.Collections.Generic;
using KalosfideAPI.Data;
using KalosfideAPI.Partages.KeyString;

namespace KalosfideAPI.Produits
{
    public class ProduitTransformation : KeyRIdNoTransformation<Produit, ProduitVue>, IProduitTransformation
    {
        public void CopieVueDansDonnées(Produit donnée, ProduitVue vue)
        {
            donnée.Nom = vue.Nom;
            donnée.Description = vue.Description;
            donnée.Unité = vue.Unité;
            donnée.QuantitéVautUnités = vue.QuantitéVautUnités;
        }

        public Produit CréeDonnée(ProduitVue vue)
        {
            Produit produit = new Produit();
            FixeKey(produit, vue);
            CopieVueDansDonnées(produit, vue);
            return produit;
        }

        public ProduitVue CréeVue(Produit donnée)
        {
            ProduitVue vue = new ProduitVue();
            FixeVueId(donnée, vue);
            return vue;
        }

        public IEnumerable<ProduitVue> CréeVues(IEnumerable<Produit> s)
        {
            throw new System.NotImplementedException();
        }
    }
}
