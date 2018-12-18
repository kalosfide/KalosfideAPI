using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Partages.KeyParams;
using System.Collections.Generic;

namespace KalosfideAPI.Catégories
{
    public class CatégorieVue: AKeyUidRnoNo
    {
        // identité
        public override string Uid { get; set; }
        public override int Rno { get; set; }
        public override long No { get; set; }

        // données
        public string Nom { get; set; }

        // calculés
        public int NbProduits { get; set; }
    }
}
