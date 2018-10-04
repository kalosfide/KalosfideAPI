using KalosfideAPI.Data.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyString
{
    public abstract class KeyStringTransformation<T, TVue> where T : AKeyBase where TVue : AKeyBase
    {
        public abstract void FixeKey(T donnée, TVue vue);
        public abstract void FixeVueId(T donnée, TVue vue);
    }
}
