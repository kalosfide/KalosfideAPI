using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Data.Keys
{
    public abstract class AKeyBase
    {
        public static string Séparateur = "/";
        public abstract string TexteKey { get; }

        // vrai si même type dérivé et même texte clé
        public abstract bool EstSemblable(AKeyBase donnée);
    }
}
