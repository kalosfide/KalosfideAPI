using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Data.Keys
{
    // base des modéles (et des vues) des données crées par un Utilisateur
    // classes dérivées: Role
    public abstract class AKeyUIdRNo : AKeyString
    {
        public abstract string UtilisateurId { get; set; }
        public abstract int RoleNo { get; set; }

        public override string Key
        {
            get
            {
                return UtilisateurId + Séparateur + RoleNo;
            }
        }
        public override bool EstSemblable(AKeyString donnée)
        {
            if (donnée is AKeyUIdRNo)
            {
                AKeyUIdRNo key = (donnée as AKeyUIdRNo);
                return key.UtilisateurId == UtilisateurId && key.RoleNo == RoleNo;
            }
            return false;
        }
    }
}
