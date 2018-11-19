using KalosfideAPI.Data.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyParams
{
    public interface IKeyUidRnoNoService<T> : IKeyParamService<T, KeyParam> where T : AKeyUidRnoNo
    {
        Task<long> DernierNo(string uid, int rno);
    }
}
