using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Data.Keys
{
    public class KeyUidRnoNo2 : AKeyUidRnoNo2
    {
        public KeyUidRnoNo2(KeyParam param)
        {
            Uid = param.Uid;
            Rno = param.Rno ?? 0;
            No = param.No ?? 0;
            Uid2 = param.Uid2;
            Rno2 = param.Rno2 ?? 0;
            No2 = param.No2 ?? 0;
        }
        public override string Uid { get; set; }
        public override int Rno { get; set; }
        public override long No { get; set; }
        public override string Uid2 { get; set; }
        public override int Rno2 { get; set; }
        public override long No2 { get; set; }
    }
}
