using KalosfideAPI.Data.Constantes;
using KalosfideAPI.Data.Keys;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace KalosfideAPI.Sécurité
{

    public class CarteRole
    {
        [JsonProperty]
        public int Rno { get; set; }
        [JsonProperty]
        public string Etat { get; set; }
        [JsonProperty]
        public string NomSite { get; set; }
    }


    [JsonObject(MemberSerialization.OptIn)]
    public class CarteUtilisateur // pour l'Api et pour l'application cliente
    {
        [JsonProperty]
        public string UserId { get; set; }
        [JsonProperty]
        public string UserName { get; set; }

        [JsonProperty]
        public string Uid { get; set; }
        [JsonProperty]
        public string Etat { get; set; }

        [JsonProperty]
        public List<CarteRole> Roles { get; set; }

        public string RolesSérialisés
        {
            get
            {
                return JsonConvert.SerializeObject(Roles);
            }
            set
            {
                Roles = JsonConvert.DeserializeObject<List<CarteRole>>(value);
            }
        }

        public void PrendClaims(ClaimsPrincipal user)
        {
            IEnumerable<Claim> claims = user.Identities.FirstOrDefault()?.Claims;
            if (claims != null && claims.Count() > 0) {
                Claim claim = user.Claims.Where(c => c.Type == JwtClaims.UserId).First();
                UserId = (claims.Where(c => c.Type == JwtClaims.UserId).First())?.Value;
                UserName = (claims.Where(c => c.Type == JwtClaims.UserName).First())?.Value;
                Uid = (claims.Where(c => c.Type == JwtClaims.UtilisateurId).First())?.Value;
                Etat = (claims.Where(c => c.Type == JwtClaims.EtatUtilisateur).First())?.Value;
                RolesSérialisés = (claims.Where(c => c.Type == JwtClaims.Roles).First())?.Value;
            }
        }

        public void DonneClaims(ClaimsPrincipal user)
        {
            IEnumerable<Claim> claims = user.Claims;
            UserId = (claims.Where(c => c.Type == JwtClaims.UserId).First())?.Value;
            UserName = (claims.Where(c => c.Type == JwtClaims.UserName).First())?.Value;
            Uid = (claims.Where(c => c.Type == JwtClaims.UtilisateurId).First())?.Value;
            Etat = (claims.Where(c => c.Type == JwtClaims.EtatUtilisateur).First())?.Value;
            RolesSérialisés = (claims.Where(c => c.Type == JwtClaims.Roles).First())?.Value;
        }

        public bool EstIdentifié
        {
            get
            {
                return UserId != null;
            }
        }

        public bool EstUtilisateurActif
        {
            get
            {
                return EstIdentifié && Etat == Data.Constantes.TypeEtatUtilisateur.Actif || Etat == Data.Constantes.TypeEtatUtilisateur.Nouveau;
            }
        }

        public bool EstAdministrateur
        {
            get
            {
                return EstUtilisateurActif && Roles.Count == 0;
            }
        }

        public bool EstRoleActif(CarteRole role)
        {
                return EstUtilisateurActif && (role.Etat == TypeEtatRole.Actif || role.Etat == TypeEtatRole.Nouveau);
        }

        public bool EstPropriétaire(KeyParam param)
        {
            return EstUtilisateurActif && param.Uid == Uid
                && (param.Rno == null || Roles.Where(role => EstRoleActif(role) && role.Rno == param.Rno).Any());
        }
    }
}
