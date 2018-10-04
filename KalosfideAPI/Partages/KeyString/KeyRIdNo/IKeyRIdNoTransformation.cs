using KalosfideAPI.Data.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyString
{
    public interface IKeyRIdNoTransformation<T, TVue>: IKeyStringTransformation<T, TVue> where T : AKeyRIdNo where TVue : AKeyRIdNo
    {
    }
}
