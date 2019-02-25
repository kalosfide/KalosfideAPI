using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Data.Keys
{
    public abstract class AKeyBase
    {
        public static string Séparateur = "-";
        [JsonIgnore]
        public abstract string TexteKey { get; }

        // vrai si même type dérivé et même texte clé
        public abstract bool EstSemblable(AKeyBase donnée);

        public abstract bool EstSemblable(KeyParam param);

        public abstract void CopieKey(KeyParam param);

        [JsonIgnore]
        public abstract KeyParam KeyParam { get; }
        [JsonIgnore]
        public abstract KeyParam KeyParamParent { get; }
        [JsonIgnore]
        public virtual KeyParam KeyParamSite
        {
            get
            {
                return null;
            }
        }
    }
}
