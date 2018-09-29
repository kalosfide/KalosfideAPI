using KalosfideAPI.Sécurité;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyLong
{
    public class KeyLongAutorisation<T> : AuthorizationHandler<OperationAuthorizationRequirement, T> where T : class, IKeyLong
    {

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            T resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            var claims = context.User.Claims;
            if (RevendicationsFabrique.EstAdministrateur(claims))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
