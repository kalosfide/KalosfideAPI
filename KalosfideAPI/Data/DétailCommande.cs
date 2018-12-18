using KalosfideAPI.Data.Constantes;
using KalosfideAPI.Data.Keys;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KalosfideAPI.Data
{
    public class DétailCommande : AKeyUidRnoNo2
    {
        // key
        [Required]
        [MaxLength(LongueurMax.UId)]
        public override string Uid { get; set; }
        [Required]
        public override int Rno { get; set; }
        [Required]
        public override long No { get; set; }

        [Required]
        [MaxLength(LongueurMax.UId)]
        public override string Uid2 { get; set; }
        [Required]
        public override int Rno2 { get; set; }
        [Required]
        public override long No2 { get; set; }

        // données
        [Required]
        [StringLength(1)]
        public string TypeCommande { get; set; }
        [Column(TypeName = "decimal(5,3)")]
        public decimal Demande { get; set; }
        [Column(TypeName = "decimal(5,3)")]
        public decimal? Mesure { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? Prix { get; set; }

        // navigation
        virtual public Commande Commande { get; set; }
        virtual public Produit Produit { get; set; }

        // création
        public static void CréeTable(ModelBuilder builder)
        {
            var entité = builder.Entity<DétailCommande>();

            entité.HasKey(donnée => new
            {
                donnée.Uid,
                donnée.Rno,
                donnée.No,
                donnée.Uid2,
                donnée.Rno2,
                donnée.No2
            });

            entité
                .HasOne(dc => dc.Commande)
                .WithMany(c => c.DétailCommandes)
                .HasForeignKey(dc => new { dc.Uid, dc.Rno, dc.No })
                .HasPrincipalKey(c => new { c.Uid, c.Rno, c.No });

            entité
                .HasOne(dc => dc.Produit)
                .WithMany()
                .HasForeignKey(dc => new { dc.Uid2, dc.Rno2, dc.No2 })
                .HasPrincipalKey(p => new { p.Uid, p.Rno, p.No })
                .OnDelete(DeleteBehavior.ClientSetNull);

            entité.ToTable("DétailCommandes");
        }
    }
}