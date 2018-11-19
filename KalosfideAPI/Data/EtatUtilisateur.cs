using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Data
{
    public class EtatUtilisateur
    {
        // key
        [Required]
        [MaxLength(Constantes.LongueurMax.UId)]
        public string Uid { get; set; }
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
            var entité = builder.Entity<EtatUtilisateur>();

            entité.HasKey(donnée => new
            {
                donnée.Uid,
                donnée.Date
            });

            entité
                .HasOne(eu => eu.Utilisateur)
                .WithMany(u => u.Etats)
                .HasForeignKey(eu => eu.Uid)
                .HasPrincipalKey(u => u.Uid);

            entité.ToTable("EtatsUtilisateur");
        }
    }
}
