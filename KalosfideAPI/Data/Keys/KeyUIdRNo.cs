using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Data.Keys
{
    public class KeyUidRno : AKeyUidRno
    {
        public KeyUidRno(KeyParam param)
        {
            Uid = param.Uid;
            Rno = param.Rno ?? 0;
        }
        public override string Uid { get; set; }
        public override int Rno { get; set; }
    }
}
