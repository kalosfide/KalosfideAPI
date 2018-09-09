using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages
{
    public interface IKeyUtilisateurIdRoleNoNo : IKeyUtilisateurId
    {
        int RoleNo { get; set; }
        int No { get; set; }
    }
    public class KeyUtilisateurIdRoleNoNo : IKeyUtilisateurIdRoleNoNo
    {
        public string UtilisateurId { get; set; }
        public int RoleNo { get; set; }
        public int No { get; set; }
        bool Egale(IKeyUtilisateurIdRoleNoNo key)
        {
            return UtilisateurId == key.UtilisateurId && RoleNo == key.RoleNo && No == key.No;
        }
        KeyUtilisateurIdNo KeyParent { get => new KeyUtilisateurIdNo { UtilisateurId = UtilisateurId, No = RoleNo }; }
    }
    public static class KeyUtilisateurIdRoleNoNoFabrique
    {
        public static KeyUtilisateurIdRoleNoNo CréeKey(IKeyUtilisateurIdRoleNoNo key)
        {
            return new KeyUtilisateurIdRoleNoNo
            {
                UtilisateurId = key.UtilisateurId,
                RoleNo = key.RoleNo,
                No = key.No
            };
        }
        public static KeyUtilisateurIdRoleNoNo CréeKey(string param)
        {
            var t = param.Split('/');
            if (t.Length != 3)
            {
                return null;
            }
            return new KeyUtilisateurIdRoleNoNo
            {
                UtilisateurId = t[0],
                RoleNo = int.Parse(t[1]),
                No = int.Parse(t[2])
            };
        }
    }
}
