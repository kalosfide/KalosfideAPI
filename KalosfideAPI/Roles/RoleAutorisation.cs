using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KalosfideAPI.Data;
using KalosfideAPI.Partages;
using KalosfideAPI.Sécurité;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace KalosfideAPI.Roles
{
    public class RoleAutorisation : AuthorizationHandler<OperationAuthorizationRequirement, Role>
    {

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            Role resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            var claims = context.User.Claims;

            BaseAutorisation.AutorisePropriétaire(context,
                requirement, RoleActions.AutoriséesPourLePropriétaire(),
                resource, claims);

            if (context.HasSucceeded)
            {
                return Task.CompletedTask;
            }
            switch (requirement.Name)
            {
                case RoleActions.Quitte.Nom:
                    if (!BaseAutorisation.EstPropriétaire(resource,claims))
                    {
                        context.Fail();
                    }
                    break;
                case RoleActions.Accepte.Nom:
                case RoleActions.Refuse.Nom:
                    var key = RevendicationsFabrique.KeyUtilisateurIdNo(claims);
                    var estFournisseur = resource.FournisseurId == key.UtilisateurId && resource.FournisseurNo == key.No;
                    if (estFournisseur)
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        context.Fail();
                    }
                    break;
                default:
                    break;
            }


            return Task.CompletedTask;
        }
    }
}