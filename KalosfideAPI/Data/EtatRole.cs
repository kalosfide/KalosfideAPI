using System;
using System.ComponentModel.DataAnnotations;
using KalosfideAPI.Data.Constantes;
using Microsoft.EntityFrameworkCore;

namespace KalosfideAPI.Data
{
    public class EtatRole: Keys.AKeyUidRno
    {
        // key
        [Required]
        [MaxLength(LongueurMax.UId)]
        public override string Uid { get; set; }
        [Required]
        public override int Rno { get; set; }
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
            var entité = builder.Entity<EtatRole>();

            entité.HasKey(donnée => new
            {
                donnée.Uid,
                donnée.Rno,
                donnée.Date
            });

            entité
                .HasOne(er => er.Role)
                .WithMany(r => r.Etats)
                .HasForeignKey(er => new { er.Uid, er.Rno })
                .HasPrincipalKey(r => new { r.Uid, r.Rno });

            entité.ToTable("EtatRole");
        }
    }
}
