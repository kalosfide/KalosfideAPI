using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Data.Keys
{
    public class KeyUidRnoNoD : AKeyUidRnoNoD
    {
        public KeyUidRnoNoD(KeyParam param)
        {
            Uid = param.Uid;
            Rno = param.Rno ?? 0;
            No = param.No ?? 0;
            Date = param.Date ?? DateTime.Now;
        }
        public override string Uid { get; set; }
        public override int Rno { get; set; }
        public override long No { get; set; }
        public override DateTime Date { get; set; }
    }
}
