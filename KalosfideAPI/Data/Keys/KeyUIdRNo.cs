using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Data.Keys
{
    public class KeyUIdRNo : AKeyUIdRNo
    {
        public override string UtilisateurId { get; set; }
        public override int RoleNo { get; set; }
    }
}
