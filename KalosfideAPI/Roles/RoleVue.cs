using KalosfideAPI.ChangementsDeRole;
using KalosfideAPI.Data;
using KalosfideAPI.Partages;
using KalosfideAPI.Produits;
using System.Collections.Generic;
using System.Linq;

namespace KalosfideAPI.Roles
{
    public class RoleVue : IKeyUtilisateurIdNo
    {
        public string UtilisateurId { get; set; }
        public int No { get; set; }

        // données
        public string Type { get; set; }
        public string Etat { get; set; }

        public string Nom { get; set; }
        public string Adresse { get; set; }

        public string FournisseurId { get; set; }
        public long? FournisseurNo { get; set; }

    }
}
