using KalosfideAPI.Partages;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Roles
{
    public static class RoleActions
    {
        public static class Accepte
        {
            public const string Nom = "Accepte";
            public static OperationAuthorizationRequirement Requirement = new OperationAuthorizationRequirement { Name = "Accepte" };
        }
        public static class Refuse
        {
            public const string Nom = "Refuse";
            public static OperationAuthorizationRequirement Requirement = new OperationAuthorizationRequirement { Name = "Refuse" };
        }
        public static class Quitte
        {
            public const string Nom = "Quitte";
            public static OperationAuthorizationRequirement Requirement = new OperationAuthorizationRequirement { Name = "Quitte" };
        }
        public static IEnumerable<string> AutoriséesPourLePropriétaire()
        {
            return new string[]
            {
                BaseActions.Ajoute.Nom,
                BaseActions.Lit.Nom,
                BaseActions.Edite.Nom,
                Quitte.Nom
            };
        }
    }
}