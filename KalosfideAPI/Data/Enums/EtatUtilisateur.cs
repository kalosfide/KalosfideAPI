using System;
using System.Linq;

namespace KalosfideAPI.Data.Enums
{
    public class EtatUtilisateur
    {
        public const string Nouveau = "N";
        public const string Actif = "A";
        public const string Inactif = "I";
        public const string Banni = "X";
        public static bool EstEtatDeRole(string etat)
        {
            return (new string[]
            {
                Nouveau,
                Actif,
                Inactif,
                Banni
            }).Contains(etat);
        }
    }
}
