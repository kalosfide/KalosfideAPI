using KalosfideAPI.Data.Enums;
using KalosfideAPI.Data.Keys;
using Newtonsoft.Json;

namespace KalosfideAPI.Sécurité
{
    public class Revendications
    {
        [JsonProperty("usid")]
        public string UserId { get; set; }
        [JsonProperty("utid")]
        public string UtilisateurId { get; set; }
        [JsonProperty("etut")]
        public string EtatUtilisateur { get; set; }
        [JsonProperty("rono")]
        public int RoleNo { get; set; }
        [JsonProperty("tyro")]
        public string TypeRole { get; set; }
        [JsonProperty("etro")]
        public string EtatRole { get; set; }

        public bool EstAdministrateur
        {
            get
            {
                return TypeRole == TypeDeRole.Administrateur.Code;
            }
        }

        public bool EstUtilisateurActif
        {
            get
            {
                return EtatUtilisateur == Data.Enums.EtatUtilisateur.Actif || EtatUtilisateur==Data.Enums.EtatUtilisateur.Nouveau;
            }
        }

        public bool EstRoleActif
        {
            get
            {
                return EstUtilisateurActif && (EtatRole == Data.Enums.EtatRole.Actif || EtatRole == Data.Enums.EtatRole.Nouveau);
            }
        }

        public bool EstPropriétaire(AKeyString donnée)
        {
            if (EstUtilisateurActif)
            {
                if (donnée is AKeyUId)
                {
                    var key = (donnée as AKeyUId);
                    return key.UtilisateurId == UtilisateurId;
                }
                if (EstRoleActif)
                {
                    if (donnée is AKeyUIdRNo)
                    {
                        var key = (donnée as AKeyUIdRNo);
                        return key.UtilisateurId == UtilisateurId && key.RoleNo == RoleNo;
                    }
                    if (donnée is AKeyUIdRNoNo)
                    {
                        var key = (donnée as AKeyUIdRNoNo);
                        return key.UtilisateurId == UtilisateurId && key.RoleNo == RoleNo;
                    }
                }
            }
            return false;
        }

        public bool PeutDevenirPropriétaire(AKeyString donnée)
        {
            if (donnée is AKeyUIdRNo)
            {
                var key = (donnée as AKeyUIdRNo);
                return EstUtilisateurActif && key.UtilisateurId == UtilisateurId;
            }
            if (donnée is AKeyUIdRNoNo)
            {
                var key = (donnée as AKeyUIdRNoNo);
                return EstRoleActif && key.UtilisateurId == UtilisateurId && key.RoleNo == RoleNo;
            }
            return false;
        }
    }
}
