using KalosfideAPI.Data.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyParams
{
    public interface IKeyUidRnoNoDService<T, TVue> : IKeyParamService<T, TVue, KeyParam> where T : AKeyUidRnoNoD where TVue : AKeyUidRnoNoD
    {
    }
}
