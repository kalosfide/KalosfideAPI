using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KalosfideAPI.Data;

namespace KalosfideAPI.Fournisseurs
{
    public class FournisseurTransformation : IFournisseurTransformation
    {
        public void CopieVueDansDonnées(Fournisseur donnée, FournisseurVue vue)
        {
            donnée.Nom = vue.Nom;
            donnée.Adresse = vue.Adresse;
        }

        public Fournisseur CréeDonnée(FournisseurVue vue)
        {
            return new Fournisseur
            {
                Uid = vue.Uid,
                Rno = vue.Rno,
                Nom = vue.Nom,
                Adresse = vue.Adresse,
            };
        }

        public FournisseurVue CréeVue(Fournisseur donnée)
        {
            return new FournisseurVue
            {
                Uid = donnée.Uid,
                Rno = donnée.Rno,
                Nom = donnée.Nom,
                Adresse = donnée.Adresse,
            };
        }

        public IEnumerable<FournisseurVue> CréeVues(IEnumerable<Fournisseur> données)
        {
            List<FournisseurVue> vues = new List<FournisseurVue>();
            foreach (Fournisseur donnée in données)
            {
                vues.Add(CréeVue(donnée));
            }
            return vues;
        }
    }
}
