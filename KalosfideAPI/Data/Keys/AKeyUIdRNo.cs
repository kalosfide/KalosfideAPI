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

        public string RoleKey
        {
            get
            {
                return UtilisateurId + Séparateur + RoleNo;
            }
            set
            {
                KeyFabrique fabrique = new KeyFabrique(value);
                if (fabrique.KeyUIdRNo == null)
                {
                    throw new ArgumentException("Mausaise key");
                }
                UtilisateurId = fabrique.KeyUIdRNo.UtilisateurId;
                RoleNo = fabrique.KeyUIdRNo.RoleNo;
            }
        }

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
