using KalosfideAPI.Data;
using KalosfideAPI.Utilisateurs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KalosfideAPI.Sécurité
{
    public class FixeClaimsMiddelware : IMiddleware
    {
        private readonly IJwtFabrique _jwtFabrique;

        public FixeClaimsMiddelware(IJwtFabrique jwtFabrique)
        {
            _jwtFabrique = jwtFabrique;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var user = context.User;
            Utilisateur utilisateurAvecRoleSelectionné = await _service.UtilisateurAvecRoleSelectionné(user);
            JwtRéponse jwtRéponse = await _jwtFabrique.CréeReponse(user, utilisateurAvecRoleSelectionné);
            var result = new OkObjectResult(jwtRéponse);

            await next(context);
        }
    }
}
