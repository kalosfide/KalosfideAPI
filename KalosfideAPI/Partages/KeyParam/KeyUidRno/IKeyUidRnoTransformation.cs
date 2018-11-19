using KalosfideAPI.Data.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyParams
{
    public interface IKeyUidRnoTransformation<T, TVue> : IKeyParamTransformation<T, TVue> where T : AKeyUidRno where TVue : AKeyUidRno
    {
    }
}
