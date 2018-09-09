using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages
{
    public static class BaseActions
    {
        public static class Ajoute
        {
            public const string Nom = "Ajoute";
            public static OperationAuthorizationRequirement Requirement = new OperationAuthorizationRequirement { Name = "Ajoute" };
        }
        public static class Lit
        {
            public const string Nom = "Lit";
            public static OperationAuthorizationRequirement Requirement = new OperationAuthorizationRequirement { Name = "Lit" };
        }
        public static class Edite
        {
            public const string Nom = "Edite";
            public static OperationAuthorizationRequirement Requirement = new OperationAuthorizationRequirement { Name = "Edite" };
        }
        public static class Supprime
        {
            public const string Nom = "Supprime";
            public static OperationAuthorizationRequirement Requirement = new OperationAuthorizationRequirement { Name = "Supprime" };
        }
    }
}