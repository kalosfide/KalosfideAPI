using KalosfideAPI.ChangementsDeRole;
using KalosfideAPI.Data;
using KalosfideAPI.Data.Constantes;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Partages.KeyString;
using KalosfideAPI.Produits;
using System.Collections.Generic;
using System.Linq;

namespace KalosfideAPI.Roles
{
    public class RoleVue : AKeyUIdRNo
    {
        public override string UtilisateurId { get; set; }
        public override int RoleNo { get; set; }

        // données
        public string Type { get; set; }

        // utiles
        public string AdministrateurId
        {
            get
            {
                return Type == TypeDeRole.Administrateur.Code ? RoleId : null;
            }
        }
        public string FournisseurId
        {
            get
            {
                return Type == TypeDeRole.Fournisseur.Code ? RoleId : null;
            }
        }
        public string ClientId
        {
            get
            {
                return Type == TypeDeRole.Client.Code ? RoleId : null;
            }
        }
    }
}
