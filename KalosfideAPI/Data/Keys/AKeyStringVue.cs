using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Data.Keys
{
    public class KeyStringVue: AKeyString
    {
        public string VueId { get; set; }
        public override string Key
        {
            get
            {
                return VueId;
            }
        }
        public override bool EstSemblable(AKeyString donnée)
        {
            return VueId == donnée.Key;
        }
    }
}
