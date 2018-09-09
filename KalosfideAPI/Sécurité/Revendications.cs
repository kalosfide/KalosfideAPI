using KalosfideAPI.Data;
using KalosfideAPI.Partages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace KalosfideAPI.Sécurité
{
    public class Revendications
    {
        public string UtilisateurId;
        public string EtatUtilisateur;
        public int RoleNo;
        public string TypeRole;
        public string EtatRole;
    }

    public static class ClaimsDuJeton
    {
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

        public static bool EstAdministrateur(ClaimsPrincipal utilisateur)
        {
            Claim claim = utilisateur.Claims.Where(c => c.Type == ClaimsDuJeton.TypeRole).FirstOrDefault();
            if (claim!=null)
            {
                return claim.Value == TypeDeRole.Administrateur.Code;
            }
            return false;
        }

        public static KeyUtilisateurIdNo KeyUtilisateurIdNo(IEnumerable<Claim> claims)
        {
            string utilisateurId = UtilisateurId(claims);
            if (utilisateurId != null)
            {
                Claim claim = claims.Where(c => c.Type == ClaimsDuJeton.RoleNo).First();
                string roleNo = claim?.Value;
                if (roleNo != null)
                {
                    return new KeyUtilisateurIdNo { UtilisateurId = utilisateurId, No = int.Parse(roleNo) };
                }
            }
            return null;
        }
    }
}
