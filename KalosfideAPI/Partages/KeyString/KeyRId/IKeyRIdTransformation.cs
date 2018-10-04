using KalosfideAPI.Data.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyString
{
    public interface IKeyRIdTransformation<T, TVue>: IKeyStringTransformation<T, TVue> where T : AKeyRId where TVue : AKeyRId
    {
    }
}
