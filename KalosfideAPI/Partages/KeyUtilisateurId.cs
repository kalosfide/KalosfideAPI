using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages
{
    public interface IKeyUtilisateurId
    {
        string UtilisateurId { get; set; }
    }
    public class KeyUtilisateurId : IKeyUtilisateurId
    {
        public string UtilisateurId { get; set; }
    }
    public static class KeyUtilisateurIdFabrique
    {
        public static KeyUtilisateurId CréeKey(IKeyUtilisateurId key)
        {
            return new KeyUtilisateurId
            {
                UtilisateurId = key.UtilisateurId,
            };
        }
        public static KeyUtilisateurId CréeKey(string param)
        {
            var t = param.Split('/');
            if (t.Length != 1)
            {
                return null;
            }
            return new KeyUtilisateurId
            {
                UtilisateurId = t[0],
            };
        }
    }
}
