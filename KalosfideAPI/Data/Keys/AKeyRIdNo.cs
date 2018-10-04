using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Data.Keys
{
    // base des modéles (et des vues) des données crées par un Role
    // classes dérivées: Produit, Commande, ... 
    public abstract class AKeyRIdNo : AKeyBase
    {
        public abstract string RoleId { get; set; }
        public abstract long No { get; set; }

        public override string TexteKey
        {
            get
            {
                return RoleId + Séparateur + No;
            }
        }
        public override bool EstSemblable(AKeyBase donnée)
        {
            if (donnée is AKeyRIdNo)
            {
                AKeyRIdNo key = (donnée as AKeyRIdNo);
                return key.RoleId == RoleId && key.No == No;
            }
            return false;
        }

    }
}
