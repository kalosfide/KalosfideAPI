using KalosfideAPI.Data.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyString.KeyUIdRNoNo
{
    public interface IKeyUIdRNoNoService<T> : IKeyStringService<T> where T : AKeyUIdRNoNo
    {
        Task<long> DernierNo(AKeyUIdRNo key);
    }
}
