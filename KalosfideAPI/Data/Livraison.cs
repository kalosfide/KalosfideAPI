using KalosfideAPI.Partages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KalosfideAPI.Data
{
    public class Livraison : IKeyUtilisateurIdRoleNoNo
    {
        // key
        [Required]
        [MaxLength(LongueurUtilisateurId.Max)]
        public string UtilisateurId { get; set; }
        [Required]
        public int RoleNo { get; set; }
        [Required]
        public int No { get; set; }

        // données
        [Required]
        public DateTime Date { get; set; }

        // navigation
        virtual public ICollection<Commande> Commandes { get; set; }
        virtual public Role Livreur { get; set; }

        // création
        public static void CréeTable(ModelBuilder builder)
        {
            var entité = builder.Entity<Livraison>();

            entité.HasKey(donnée => new
            {
                donnée.UtilisateurId,
                donnée.RoleNo,
                donnée.No
            });

            entité.HasIndex(donnée => donnée.Date);

            entité
                .HasOne(livraison => livraison.Livreur)
                .WithMany(livreur => livreur.Livraisons)
                .HasForeignKey(livraison => new
                {
                    livraison.UtilisateurId,
                    livraison.RoleNo
                });

            entité.ToTable("Livraisons");
        }
    }
}
