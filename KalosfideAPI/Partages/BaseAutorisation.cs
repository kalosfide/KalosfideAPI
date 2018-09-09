using KalosfideAPI.Data;
using KalosfideAPI.Sécurité;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages
{
    public static class BaseAutorisation
    {

        public static bool EstPropriétaire<T>(T resource, IEnumerable<Claim> claims) where T : class, IKeyUtilisateurIdNo
        {
            if (resource is IKeyUtilisateurIdRoleNoNo)
            {
                var key = KeyUtilisateurIdNoFabrique.CréeKey(RevendicationsFabrique.KeyUtilisateurIdNo(claims));
                return key.UtilisateurId == resource.UtilisateurId && key.No == (resource as IKeyUtilisateurIdRoleNoNo).RoleNo;
            }

            if (resource is IKeyUtilisateurIdNo)
            {
                var key = new KeyUtilisateurId { UtilisateurId = RevendicationsFabrique.UtilisateurId(claims) };
                return key.UtilisateurId == resource.UtilisateurId;
            }

            return false;
        }

        public static Task AutorisePropriétaire<T>(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            IEnumerable<string> opérationsAutorisées,
            T resource,
            IEnumerable<Claim> claims) where T : class, IKeyUtilisateurIdNo
        {
            if (EstPropriétaire(resource, claims) && opérationsAutorisées.Contains(requirement.Name))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}