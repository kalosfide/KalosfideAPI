using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Data.Keys
{
    // classes dérivées: Utilisateur
    public abstract class AKeyUId : AKeyBase
    {
        public abstract string UtilisateurId { get; set; }
        public override string TexteKey
        {
            get
            {
                return UtilisateurId;
            }
        }
        public override bool EstSemblable(AKeyBase donnée)
        {
            if (donnée is AKeyUId)
            {
                return (donnée as AKeyUId).UtilisateurId == UtilisateurId;
            }
            return false;
        }
    }
}
