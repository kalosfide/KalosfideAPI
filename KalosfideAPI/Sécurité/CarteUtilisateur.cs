using KalosfideAPI.Data;
using KalosfideAPI.Data.Constantes;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Sites;
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

        [JsonProperty]
        public List<SiteVue> Sites { get; set; }

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

        public bool EstClient(string nomSite)
        {
            return EstUtilisateurActif && Roles.Where(role => EstRoleActif(role) && role.NomSite == nomSite).Any();
        }

        public SiteVue SiteClient(KeyParam siteParam)
        {
            return Sites.Where(site => site.Uid != Uid && site.EstSemblable(siteParam)).FirstOrDefault();
        }

        public SiteVue SiteFournisseur(KeyParam siteParam)
        {
            return siteParam.Uid == Uid ? Sites.Where(site => site.Uid == Uid).FirstOrDefault() : null;
        }
    }
}
