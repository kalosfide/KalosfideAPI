using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KalosfideAPI.Data
{
    public class ChangementEtatRole
    {
        // key
        [Required]
        [MaxLength(LongueurUtilisateurId.Max)]
        public string UtilisateurId { get; set; }
        [Required]
        public int RoleNo { get; set; }
        [Required]
        public DateTime Date { get; set; }

        // données
        [StringLength(1)]
        public string Etat { get; set; }

        // navigation
        virtual public Role Role { get; set; }

        // création
        public static void CréeTable(ModelBuilder builder)
        {
            var entité = builder.Entity<ChangementEtatRole>();

            entité.HasKey(donnée => new
            {
                donnée.UtilisateurId,
                donnée.RoleNo,
                donnée.Date
            });

            entité
                .HasOne(cer => cer.Role)
                .WithMany(r => r.ChangementsEtat)
                .HasForeignKey(cer => new
                {
                    cer.UtilisateurId,
                    cer.RoleNo
                });

            entité.ToTable("JournalEtatRole");
        }
    }
}
