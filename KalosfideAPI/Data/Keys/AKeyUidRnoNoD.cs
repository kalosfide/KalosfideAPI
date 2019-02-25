using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Data.Keys
{
    public abstract class AKeyUidRnoNoD : AKeyBase
    {
        public abstract string Uid { get; set; }
        public abstract int Rno { get; set; }
        public abstract long No { get; set; }
        public abstract DateTime Date { get; set; }

        public override string TexteKey
        {
            get
            {
                return Uid + Séparateur + Rno + Séparateur + No + Séparateur + Date.ToString();
            }
        }

        public override bool EstSemblable(AKeyBase donnée)
        {
            if (donnée is AKeyUidRnoNoD)
            {
                AKeyUidRnoNoD key = (donnée as AKeyUidRnoNoD);
                return Uid == key.Uid && Rno == key.Rno && No == key.No && Date == key.Date;
            }
            return false;
        }

        public override bool EstSemblable(KeyParam param)
        {
            return Uid == param.Uid && Rno == param.Rno && No == param.No && Date == Date;
        }

        public override void CopieKey(KeyParam param)
        {
            Uid = param.Uid;
            Rno = param.Rno ?? 0;
            No = param.No ?? 0;
            Date = param.Date?? DateTime.MinValue;
        }

        public override KeyParam KeyParam => new KeyParam { Uid = Uid, Rno = Rno, No = No, Date = Date };
        public override KeyParam KeyParamParent => new KeyParam { Uid = Uid, Rno = Rno, No = No };
    }
}
