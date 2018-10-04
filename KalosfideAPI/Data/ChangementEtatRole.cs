using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KalosfideAPI.Data
{
    public class ChangementEtatRole: Keys.AKeyRId
    {
        // key
        [Required]
        [MaxLength(Constantes.LongueurMax.RoleId)]
        public override string RoleId { get; set; }
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
                donnée.RoleId,
                donnée.Date
            });

            entité
                .HasOne(cer => cer.Role)
                .WithMany(r => r.ChangementsEtat)
                .HasForeignKey(cer => cer.RoleId)
                .HasPrincipalKey(role => role.RoleId);

            entité.ToTable("JournalEtatRole");
        }
    }
}
