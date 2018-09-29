using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Data.Keys
{
    public abstract class AKeyString
    {
        public static string Séparateur = "/";
        public abstract string Key { get; }
        public abstract bool EstSemblable(AKeyString donnée);
    }
}
