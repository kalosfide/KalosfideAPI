using KalosfideAPI.Data.Constantes;
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
                return EtatUtilisateur == Data.Constantes.EtatUtilisateur.Actif || EtatUtilisateur==Data.Constantes.EtatUtilisateur.Nouveau;
            }
        }

        public bool EstRoleActif
        {
            get
            {
                return EstUtilisateurActif && (EtatRole == Data.Constantes.EtatRole.Actif || EtatRole == Data.Constantes.EtatRole.Nouveau);
            }
        }

        public bool EstPropriétaire(AKeyBase donnée)
        {
            if (EstUtilisateurActif)
            {
                if (donnée is AKeyUId)
                {
                    AKeyUId aKey = (donnée as AKeyUId);
                    return aKey.UtilisateurId == UtilisateurId;
                }
                if (EstRoleActif)
                {
                    AKeyUIdRNo aKey = null;
                    if (donnée is AKeyUIdRNo)
                    {
                        aKey = (donnée as AKeyUIdRNo);
                    }
                    else
                    {
                        if (donnée is AKeyRId)
                        {
                            aKey = KeyFabrique.CréeKeyUIdRNo((donnée as AKeyRId).RoleId);
                        }
                        else
                        {
                            if (donnée is AKeyRIdNo)
                            {
                                aKey = KeyFabrique.CréeKeyUIdRNo((donnée as AKeyRIdNo).RoleId);
                            }
                        }
                    }
                    return aKey.UtilisateurId == UtilisateurId && aKey.RoleNo == RoleNo;
                }
            }
            return false;
        }

        public bool PeutDevenirPropriétaire(AKeyBase donnée)
        {
            if (EstUtilisateurActif)
            {
                AKeyUIdRNo aKey = null;
                if (donnée is AKeyUIdRNo)
                {
                    aKey = (donnée as AKeyUIdRNo);
                    return aKey.UtilisateurId == UtilisateurId;
                }
                else
                {
                    if (donnée is AKeyRId)
                    {
                        aKey = KeyFabrique.CréeKeyUIdRNo((donnée as AKeyRId).RoleId);
                        return aKey.UtilisateurId == UtilisateurId;
                    }
                    else
                    {
                        if (donnée is AKeyRIdNo)
                        {
                            aKey = KeyFabrique.CréeKeyUIdRNo((donnée as AKeyRIdNo).RoleId);
                            return aKey.UtilisateurId == UtilisateurId && aKey.RoleNo == RoleNo;
                        }
                    }
                }
            }
            return false;
        }
    }
}
