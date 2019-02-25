using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KalosfideAPI.Data.Constantes;
using Microsoft.EntityFrameworkCore;

namespace KalosfideAPI.Data
{
    public class Produit: Keys.AKeyUidRnoNo
    {
        // key
        [Required]
        [MaxLength(LongueurMax.UId)]
        public override string Uid { get; set; }
        [Required]
        public override int Rno { get; set; }
        [Required]
        public override long No { get; set; }

        // données
        [Required]
        [MinLength(10), MaxLength(200)]
        public string Nom { get; set; }
        [Required]
        public long CategorieNo { get; set; }

        [Required]
        [StringLength(1)]
        public string TypeMesure { get; set; }
        [Required]
        [StringLength(1)]
        public string TypeCommande { get; set; }
        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal Prix { get; set; }

        // navigation
        virtual public Catégorie Catégorie { get; set; }

        // utiles

        // création
        public static void CréeTable(ModelBuilder builder)
        {
            var entité = builder.Entity<Produit>();

            entité.HasKey(donnée => new { donnée.Uid, donnée.Rno, donnée.No });

            entité.HasIndex(donnée => donnée.Nom).IsUnique();

            entité.Property(donnée => donnée.TypeMesure).HasDefaultValue(TypeUnitéDeMesure.Aucune);
            entité.Property(donnée => donnée.TypeCommande).HasDefaultValue(TypeUnitéDeCommande.Unité);
            entité.Property(donnée => donnée.Prix).HasDefaultValue(0);

            entité
                .HasOne(produit => produit.Catégorie)
                .WithMany(catégorie => catégorie.Produits)
                .HasForeignKey(produit => new { produit.Uid, produit.Rno, produit.CategorieNo })
                .HasPrincipalKey(catégorie => new { catégorie.Uid, catégorie.Rno, catégorie.No });

            entité.ToTable("Produits");
        }
    }
}
