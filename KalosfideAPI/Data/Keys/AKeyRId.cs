using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Data.Keys
{
    // base des modéles (et des vues) des données crées par un Role
    // classes dérivées: client, ...
    public abstract class AKeyRId : AKeyBase
    {
        public abstract string RoleId { get; set; }

        public override string TexteKey
        {
            get
            {
                return RoleId;
            }
        }
        public override bool EstSemblable(AKeyBase donnée)
        {
            if (donnée is AKeyRId)
            {
                AKeyRId key = (donnée as AKeyRId);
                return key.RoleId == RoleId;
            }
            return false;
        }
    }
}
