using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace KalosfideAPI.Autorisation
{
    public static class Actions
    {
        public static class Utilisateur
        {
            public static class Enregistre
            {
                public const string Nom = "Enregistre";
                public static OperationAuthorizationRequirement Requirement = new OperationAuthorizationRequirement { Name = "Enregistre" };
            }
            public static class Connecte
            {
                public const string Nom = "Connecte";
                public static OperationAuthorizationRequirement Requirement = new OperationAuthorizationRequirement { Name = "Connecte" };
            }
        }
    }
}