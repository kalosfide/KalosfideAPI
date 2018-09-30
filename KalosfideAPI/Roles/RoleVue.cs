using KalosfideAPI.ChangementsDeRole;
using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Partages;
using KalosfideAPI.Produits;
using System.Collections.Generic;
using System.Linq;

namespace KalosfideAPI.Roles
{
    public class RoleVue : AKeyUIdRNoVue
    {

        // données
        public string Type { get; set; }

        public string Nom { get; set; }
        public string Adresse { get; set; }

        public string FournisseurKey { get; set; }

    }
}
