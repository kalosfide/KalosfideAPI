using KalosfideAPI.Commandes;
using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using System;
using System.Collections.Generic;

namespace KalosfideAPI.Livraisons
{
    public class LivraisonVue : AKeyUidRnoNo
    {
        // key
        public override string Uid { get; set; }
        public override int Rno { get; set; }
        public override long No { get; set; }

        // données
        public DateTime? Date { get; set; }

        // navigation
        virtual public List<CommandeVue> Commandes { get; set; }
        virtual public Fournisseur Livreur { get; set; }
    }
}
