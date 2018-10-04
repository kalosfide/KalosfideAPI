using KalosfideAPI.Data.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyString
{
    public interface IKeyRIdNoService<T> : IKeyStringService<T> where T : AKeyRIdNo
    {
        Task<long> DernierNo(AKeyRIdNo key);
    }
}
