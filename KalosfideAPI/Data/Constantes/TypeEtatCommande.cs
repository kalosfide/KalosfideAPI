using System;
using System.Linq;

namespace KalosfideAPI.Data.Constantes
{
    public static class TypeEtatCommande
    {
        public const string Nouveau = "N";
        public const string Accepté = "A";
        public const string Refusé = "R";
        public const string Préparé = "P";
        public const string Facturé = "F";
        public static bool EstValide(string etat)
        {
            return (new string[]
            {
                Nouveau,
                Accepté,
                Refusé
            }).Contains(etat);
        }
    }
}
