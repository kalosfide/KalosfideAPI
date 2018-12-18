using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Data.Keys
{
    public class KeyUid : AKeyUid
    {
        public KeyUid(KeyParam param)
        {
            Uid = param.Uid;
        }

        public override string Uid { get; set; }
    }
}
