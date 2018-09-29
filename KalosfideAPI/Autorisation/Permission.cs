using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Autorisation
{
    public class Permission<T>
    {
        public string Nom { get; set; }
        public Func<T, bool> func { get; set; }
    }
}
