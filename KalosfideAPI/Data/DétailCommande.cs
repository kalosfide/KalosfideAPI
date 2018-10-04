using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace KalosfideAPI.Data
{
    public class DétailCommande
    {

        // key
        [Required]
        [MaxLength(Constantes.LongueurMax.RoleId)]
        public string CommandeClientId { get; set; }
        public long CommandeNo { get; set; }

        [Required]
        [MaxLength(Constantes.LongueurMax.RoleId)]
        public string ProduitFournisseurId { get; set; }
        public int ProduitNo { get; set; }

        // données
        public int Quantité { get; set; }
        public decimal UnitésLivrées { get; set; }

        // navigation
        virtual public Commande Commande { get; set; }
        virtual public Produit Produit { get; set; }

        // création
        public static void CréeTable(ModelBuilder builder)
        {
            var entité = builder.Entity<DétailCommande>();

            entité.HasKey(donnée => new
            {
                donnée.CommandeClientId,
                donnée.CommandeNo,
                donnée.ProduitFournisseurId,
                donnée.ProduitNo
            });

            entité
                .HasOne(dc => dc.Commande)
                .WithMany(c => c.DétailCommandes)
                .HasForeignKey(dc => new
                {
                    dc.CommandeClientId,
                    dc.CommandeNo
                })
                .HasPrincipalKey(c => new
                {
                    c.RoleId,
                    c.No
                });

            entité
                .HasOne(dc => dc.Produit)
                .WithMany()
                .HasForeignKey(dc => new
                {
                    dc.CommandeClientId,
                    dc.CommandeNo
                })
                .HasPrincipalKey(p => new
                {
                    p.RoleId,
                    p.No
                })
                .OnDelete(DeleteBehavior.ClientSetNull);

            entité.ToTable("DétailCommandes");
        }
    }
}