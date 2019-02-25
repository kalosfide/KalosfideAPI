using KalosfideAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Enregistrement
{
    public class EnregistrementFournisseurVue: VueBase
    {
        public string Nom { get; set; }
        public string Adresse { get; set; }

        public string NomSite { get; set; }
        public string Titre { get; set; }

    }
}
