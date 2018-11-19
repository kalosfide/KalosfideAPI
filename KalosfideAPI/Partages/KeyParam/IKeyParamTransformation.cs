using KalosfideAPI.Data.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyParams
{
    public interface IKeyParamTransformation<T, TVue>: ITransformation<T,TVue> where T: AKeyBase where TVue: AKeyBase
    {
    }
}
