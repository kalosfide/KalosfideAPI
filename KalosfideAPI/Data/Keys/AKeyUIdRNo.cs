using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Data.Keys
{
    // base des modéles (et des vues) des données crées par un Utilisateur
    // classes dérivées: Role
    public abstract class AKeyUIdRNo : AKeyBase
    {
        public abstract string UtilisateurId { get; set; }
        public abstract int RoleNo { get; set; }

        public string RoleId
        {
            get
            {
                return UtilisateurId + Séparateur + RoleNo;
            }
            set
            {
                KeyUIdRNo key = KeyFabrique.CréeKeyUIdRNo(value);
                if (key == null)
                {
                    throw new ArgumentException("Mausaise key");
                }
                UtilisateurId = key.UtilisateurId;
                RoleNo = key.RoleNo;
            }
        }

        public override string TexteKey
        {
            get
            {
                return UtilisateurId + Séparateur + RoleNo;
            }
        }
        public override bool EstSemblable(AKeyBase donnée)
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
