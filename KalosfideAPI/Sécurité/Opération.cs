using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages
{
    public class Opération
    {
        public string Nom { get; set; }
        public bool PermiseAuPropriétaire { get; set; } = false;
    }
}
