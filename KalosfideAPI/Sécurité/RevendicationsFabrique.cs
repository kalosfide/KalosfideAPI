using KalosfideAPI.Data;
using KalosfideAPI.Data.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace KalosfideAPI.Sécurité
{

    public static class ClaimsDuJeton
    {
        public const string UserId = "usid";
        public const string UtilisateurId = "utid";
        public const string EtatUtilisateur = "etut";
        public const string RoleNo = "rono";
        public const string TypeRole = "tyro";
        public const string EtatRole = "etro";
    }

    public static class RevendicationsFabrique
    {
        /**
         * Attention!!!
         * Utilisateur doit inclure RoleSélectionné si un role existe (RoleSélectionnéNo != 0)
         */
        public static List<Revendication> ListeRevendications(Utilisateur utilisateurAvecRoleSélectionné)
        {
            List<Revendication> revendications = new List<Revendication>
            {
                new Revendication { Nom = "UserId", JwtName = "usid", Valeur = utilisateurAvecRoleSélectionné.UserId },
                new Revendication { Nom = "UtilisateurId", JwtName = "utid", Valeur = utilisateurAvecRoleSélectionné.UtilisateurId },
                new Revendication { Nom = "EtatUtilisateur", JwtName = "etut", Valeur = utilisateurAvecRoleSélectionné.Etat },
                new Revendication { Nom = "RoleNo", JwtName = "rono", Valeur = utilisateurAvecRoleSélectionné.RoleSélectionnéNo.ToString() },
            };

            if (utilisateurAvecRoleSélectionné.RoleSélectionnéNo != 0)
            {
                revendications.Add(new Revendication { Nom = "TypeRole", JwtName = "tyro", Valeur = utilisateurAvecRoleSélectionné.RoleSélectionné.Type });
                revendications.Add(new Revendication { Nom = "EtatRole", JwtName = "etro", Valeur = utilisateurAvecRoleSélectionné.RoleSélectionné.Etat });
            }
            return revendications;
        }

        public static List<Revendication> ListeRevendications(Revendications revendications)
        {
            List<Revendication> listeRevendications = new List<Revendication>
            {
                new Revendication { Nom = "UserId", JwtName = "usid", Valeur = revendications.UserId },
                new Revendication { Nom = "UtilisateurId", JwtName = "utid", Valeur = revendications.UtilisateurId },
                new Revendication { Nom = "EtatUtilisateur", JwtName = "etut", Valeur = revendications.EtatUtilisateur },
                new Revendication { Nom = "RoleNo", JwtName = "rono", Valeur = revendications.RoleNo.ToString() },
            };

            if (revendications.RoleNo != 0)
            {
                listeRevendications.Add(new Revendication { Nom = "TypeRole", JwtName = "tyro", Valeur = revendications.TypeRole });
                listeRevendications.Add(new Revendication { Nom = "EtatRole", JwtName = "etro", Valeur = revendications.EtatRole });
            }
            return listeRevendications;
        }

        public static Revendications Revendications(Utilisateur utilisateurAvecRoleSélectionné)
        {
            Revendications revendications = new Revendications
            {
                UtilisateurId = utilisateurAvecRoleSélectionné.UtilisateurId,
                EtatUtilisateur = utilisateurAvecRoleSélectionné.Etat,
                RoleNo = utilisateurAvecRoleSélectionné.RoleSélectionnéNo
            };
            if (utilisateurAvecRoleSélectionné.RoleSélectionnéNo != 0)
            {
                revendications.TypeRole = utilisateurAvecRoleSélectionné.RoleSélectionné.Type;
                revendications.EtatRole = utilisateurAvecRoleSélectionné.RoleSélectionné.Etat;
            }
            return revendications;
        }

        // retourne la liste des claims à inclure dans le Jwt
        public static List<Claim> Claims(Revendications revendications)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimsDuJeton.UtilisateurId, revendications.UtilisateurId),
                new Claim(ClaimsDuJeton.EtatUtilisateur, revendications.EtatUtilisateur),
                new Claim(ClaimsDuJeton.RoleNo, revendications.RoleNo.ToString()),
            };
            if (revendications.RoleNo != 0)
            {
                claims.Add(new Claim(ClaimsDuJeton.EtatRole, revendications.EtatRole));
                claims.Add(new Claim(ClaimsDuJeton.TypeRole, revendications.TypeRole));
            }
            return claims;
        }

        public static Revendications Revendications(ClaimsPrincipal user)
        {
            IEnumerable<Claim> claims = user.Claims;
            Revendications revendications = new Revendications
            {
                UtilisateurId = (claims.Where(c => c.Type == ClaimsDuJeton.UtilisateurId).First())?.Value,
                EtatUtilisateur = (claims.Where(c => c.Type == ClaimsDuJeton.EtatUtilisateur).First())?.Value,
                EtatRole = (claims.Where(c => c.Type == ClaimsDuJeton.EtatRole).First())?.Value,
                TypeRole = (claims.Where(c => c.Type == ClaimsDuJeton.TypeRole).First())?.Value,
            };
            string sRoleNo = (claims.Where(c => c.Type == ClaimsDuJeton.RoleNo).First())?.Value;
            revendications.RoleNo = sRoleNo != null ? int.Parse(sRoleNo) : 0;
            return revendications;
        }

        public static string UtilisateurId(IEnumerable<Claim> claims)
        {
            Claim claim = claims.Where(c => c.Type == ClaimsDuJeton.UtilisateurId).First();
            return claim?.Value;
        }
        public static string EtatUtilisateur(IEnumerable<Claim> claims)
        {
            Claim claim = claims.Where(c => c.Type == ClaimsDuJeton.EtatUtilisateur).First();
            return claim?.Value;
        }
        public static string RoleNo(IEnumerable<Claim> claims)
        {
            Claim claim = claims.Where(c => c.Type == ClaimsDuJeton.RoleNo).First();
            return claim?.Value;
        }
        public static string TypeRole(IEnumerable<Claim> claims)
        {
            Claim claim = claims.Where(c => c.Type == ClaimsDuJeton.TypeRole).First();
            return claim?.Value;
        }
        public static string EtatRole(IEnumerable<Claim> claims)
        {
            Claim claim = claims.Where(c => c.Type == ClaimsDuJeton.EtatRole).First();
            return claim?.Value;
        }

        public static bool EstAdministrateur(IEnumerable<Claim> claims)
        {
            Claim claim = claims.Where(c => c.Type == ClaimsDuJeton.TypeRole).FirstOrDefault();
            if (claim != null)
            {
                return claim.Value == TypeDeRole.Administrateur.Code;
            }
            return false;
        }
    }
}
