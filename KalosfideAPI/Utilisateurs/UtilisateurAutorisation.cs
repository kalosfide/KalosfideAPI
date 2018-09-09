using KalosfideAPI.Data;
using KalosfideAPI.Sécurité;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Utilisateurs
{
    public class UtilisateurAutorisation : AuthorizationHandler<OperationAuthorizationRequirement, Utilisateur>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UtilisateurAutorisation(UserManager<ApplicationUser>
            userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                   OperationAuthorizationRequirement requirement,
                                   Utilisateur resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            var claims = context.User.Claims;
            var utilisateurId = RevendicationsFabrique.UtilisateurId(claims);
            var estPropriétaire = resource.UtilisateurId == utilisateurId;
            if (estPropriétaire)
            {
                switch (requirement.Name)
                {
                    case Actions.Noms.CreateOperationName:
                    case Actions.Noms.ReadOperationName:
                    case Actions.Noms.UpdateOperationName:
                        context.Succeed(requirement);
                        break;
                    default:
                        break;
                }
            }
            return Task.CompletedTask;

        }
    }
}
