using KalosfideAPI.Partages;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace KalosfideAPI.Data
{
    public class Produit : IKeyUtilisateurIdRoleNoNo
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
        [MinLength(10), MaxLength(200)]
        public string Nom { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public bool Indisponible { get; set; }

        // navigation
        virtual public Role Producteur { get; set; }

        // création
        public static void CréeTable(ModelBuilder builder)
        {
            var entité = builder.Entity<Produit>();

            entité.HasKey(donnée => new
            {
                donnée.UtilisateurId,
                donnée.RoleNo,
                donnée.No
            });

            entité.HasIndex(donnée => donnée.Nom).IsUnique();

            entité
                .HasOne(p => p.Producteur)
                .WithMany(r => r.Produits)
                .HasForeignKey(p => new
                {
                    p.UtilisateurId,
                    p.RoleNo
                });

            entité.ToTable("Produits");
        }
    }
}
