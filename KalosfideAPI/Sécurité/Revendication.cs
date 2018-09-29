using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KalosfideAPI.Sécurité
{
    public class Revendication
    {
        public string Nom { get; set; }
        public string JwtName { get; set; }
        public string Valeur { get; set; }

        public Claim Claim { get => new Claim(Nom, Valeur); }

        public Claim JwtClaim { get => new Claim(JwtName, Valeur); }

        public void FixeValeur(IEnumerable<Claim> claims)
        {
            Claim claim = claims.FirstOrDefault(c => c.Type == JwtName);
            Valeur = claim?.Value;
        }
    }
}
