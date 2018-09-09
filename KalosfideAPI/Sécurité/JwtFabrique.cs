using KalosfideAPI.Data;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace KalosfideAPI.Sécurité
{
    public class JwtRéponse
    {
        public string Id;
        public string Jeton;
        public long ExpireDans;

        public Revendications Revendications;
    }

    public class JwtFabrique : IJwtFabrique
    {
        private readonly JwtFabriqueOptions _jwtOptions;

        public JwtFabrique(IOptions<JwtFabriqueOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
        }

        public JwtRéponse CréeReponse(ApplicationUser user, Utilisateur utilisateurAvecRoleSelectionné)
        {
            Revendications revendications = RevendicationsFabrique.Revendications(utilisateurAvecRoleSelectionné);
            string jeton = CréeJeton(RevendicationsFabrique.Claims(revendications));

            JwtRéponse jwtr = new JwtRéponse
            {
                Id = user.Id,
                Jeton = jeton,
                ExpireDans = (int)_jwtOptions.ValidFor.TotalSeconds,
                Revendications = revendications
            };
            return jwtr;
        }

        private string CréeJeton(List<Claim> claims)
        {
            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims.ToArray(),
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwt);

        }

        // Utilités

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);

        private static void ThrowIfInvalidOptions(JwtFabriqueOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtFabriqueOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtFabriqueOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtFabriqueOptions.JtiGenerator));
            }
        }
    }
}