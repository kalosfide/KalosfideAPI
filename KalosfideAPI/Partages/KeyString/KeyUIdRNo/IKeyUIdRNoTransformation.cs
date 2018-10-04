using KalosfideAPI.Data.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyString
{
    public interface IKeyUIdRNoTransformation<T, TVue> : IKeyStringTransformation<T, TVue> where T : AKeyUIdRNo where TVue : AKeyUIdRNo
    {
    }
}
