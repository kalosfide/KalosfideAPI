using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Data
{
    public class ChangementEtatUtilisateur
    {
        // key
        [Required]
        [MaxLength(LongueurUtilisateurId.Max)]
        public string UtilisateurId { get; set; }
        [Required]
        public DateTime Date { get; set; }

        // données
        [StringLength(1)]
        public string Etat { get; set; }

        // navigation
        virtual public Utilisateur Utilisateur { get; set; }

        // création
        public static void CréeTable(ModelBuilder builder)
        {
            var entité = builder.Entity<ChangementEtatUtilisateur>();

            entité.HasKey(donnée => new
            {
                donnée.UtilisateurId,
                donnée.Date
            });

            entité
                .HasOne(ceu => ceu.Utilisateur)
                .WithMany(u => u.ChangementsEtat)
                .HasForeignKey(ceu => new { ceu.UtilisateurId, });

            entité.ToTable("JournalEtatUtilisateur");
        }
    }
}
