using KalosfideAPI.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KalosfideAPI.Data
{
    public static class LongueurUtilisateurId
    {
        public const int Max = 20; //  UInt64.MaxValue.ToString().Length;
    }

    public class Utilisateur
    {
        [Required]
        [MaxLength(LongueurUtilisateurId.Max)]
        public string UtilisateurId { get; set; }

        // données
        [StringLength(1)]
        [DefaultValue(EtatUtilisateur.Nouveau)]
        public string Etat { get; set; }

        [Required]
        public string UserId { get; set; }

        public int RoleSélectionnéNo { get; set; }

        // navigation
        virtual public ApplicationUser ApplicationUser { get; set; }
        virtual public Role RoleSélectionné { get; set; }
        virtual public ICollection<Role> Roles { get; set; }
        virtual public ICollection<ChangementEtatUtilisateur> ChangementsEtat { get; set; }

        // création
        public static void CréeTable(ModelBuilder builder)
        {
            var entité = builder.Entity<Utilisateur>();

            entité.HasKey(utilisateur => utilisateur.UtilisateurId);

            entité.ToTable("Utilisateurs");
        }
    }

}
