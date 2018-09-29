using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Data.Keys
{
    // base des modéles (et des vues) des données crées par un Role
    // classes dérivées: 
    public abstract class AKeyUIdRNoNo : AKeyString
    {
        public abstract string UtilisateurId { get; set; }
        public abstract int RoleNo { get; set; }
        public abstract long No { get; set; }

        public override string Key
        {
            get
            {
                return UtilisateurId + Séparateur + RoleNo + Séparateur + No;
            }
        }
        public override bool EstSemblable(AKeyString donnée)
        {
            if (donnée is AKeyUIdRNoNo)
            {
                AKeyUIdRNoNo key = (donnée as AKeyUIdRNoNo);
                return key.UtilisateurId == UtilisateurId && key.RoleNo == RoleNo && key.No == No;
            }
            return false;
        }

    }
}
