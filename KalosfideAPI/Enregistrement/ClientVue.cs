using KalosfideAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Enregistrement
{
    public class ClientVue: VueBase
    {
        public string Nom { get; set; }
        public string Adresse { get; set; }

        public string SiteUid { get; set; }
        public int SiteRno { get; set; }

        public Client CréeClient()
        {
            return new Client
            {
                Nom = Nom,
                Adresse = Adresse,
            };
        }
    }
}
