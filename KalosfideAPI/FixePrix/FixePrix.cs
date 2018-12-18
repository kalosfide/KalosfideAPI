using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.FixePrix
{
    public class FixePrix: Data.Keys.AKeyUidRnoNo
    {
        // identité
        public override string Uid { get; set; }
        public override int Rno { get; set; }
        public override long No { get; set; }
        public string Nom { get; set; }

        // calculés
        public decimal Actuel { get; set; }
        public decimal Nouveau { get; set; }
    }
}
