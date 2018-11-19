using KalosfideAPI.Data.Keys;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KalosfideAPI.Data
{
    public class DétailCommande
    {

        // key
        [Required]
        [MaxLength(Constantes.LongueurMax.UId)]
        public string CommandeUid { get; set; }
        [Required]
        public int CommandeRno { get; set; }
         [Required]
       public long CommandeNo { get; set; }

        [Required]
        [MaxLength(Constantes.LongueurMax.UId)]
        public string ProduitUId { get; set; }
        public int ProduitRno { get; set; }
        public long ProduitNo { get; set; }

        // données
        public int Quantité { get; set; }
        [Column(TypeName = "decimal(5,3)")]
        public decimal UnitésLivrées { get; set; }

        // navigation
        virtual public Commande Commande { get; set; }
        virtual public Produit Produit { get; set; }

        // utiles

        // création
        public static void CréeTable(ModelBuilder builder)
        {
            var entité = builder.Entity<DétailCommande>();

            entité.HasKey(donnée => new
            {
                donnée.CommandeUid,
                donnée.CommandeRno,
                donnée.CommandeNo,
                donnée.ProduitUId,
                donnée.ProduitRno,
                donnée.ProduitNo
            });

            entité
                .HasOne(dc => dc.Commande)
                .WithMany(c => c.DétailCommandes)
                .HasForeignKey(dc => new { dc.CommandeUid, dc.CommandeRno, dc.CommandeNo })
                .HasPrincipalKey(c => new { c.Uid, c.Rno, c.No });

            entité
                .HasOne(dc => dc.Produit)
                .WithMany()
                .HasForeignKey(dc => new { dc.ProduitUId, dc.ProduitRno, dc.ProduitNo })
                .HasPrincipalKey(p => new { p.Uid, p.Rno, p.No })
                .OnDelete(DeleteBehavior.ClientSetNull);

            entité.ToTable("DétailCommandes");
        }
    }
}