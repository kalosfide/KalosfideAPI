using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace KalosfideAPI.Data
{
    public class DétailCommande
    {

        // key
        [Required]
        [MaxLength(LongueurUtilisateurId.Max)]
        public string CommandeUtilisateurId { get; set; }
        public int CommandeRoleNo { get; set; }
        public int CommandeNo { get; set; }

        [Required]
        [MaxLength(LongueurUtilisateurId.Max)]
        public string ProduitUtilisateurId { get; set; }
        public int ProduitRoleNo { get; set; }
        public int ProduitNo { get; set; }

        // données
        public int Quantité { get; set; }

        // navigation
        virtual public Commande Commande { get; set; }
        virtual public Produit Produit { get; set; }

        // création
        public static void CréeTable(ModelBuilder builder)
        {
            var entité = builder.Entity<DétailCommande>();

            entité.HasKey(donnée => new
            {
                donnée.CommandeUtilisateurId,
                donnée.CommandeRoleNo,
                donnée.CommandeNo,
                donnée.ProduitUtilisateurId,
                donnée.ProduitRoleNo,
                donnée.ProduitNo
            });

            entité
                .HasOne(dc => dc.Commande)
                .WithMany(c => c.DétailCommandes)
                .HasForeignKey(dc => new
                {
                    dc.CommandeUtilisateurId,
                    dc.CommandeRoleNo,
                    dc.CommandeNo
                })
                .HasPrincipalKey(c => new
                {
                    c.UtilisateurId,
                    c.RoleNo,
                    c.No
                });

            entité
                .HasOne(dc => dc.Produit)
                .WithMany()
                .HasForeignKey(dc => new
                {
                    dc.CommandeUtilisateurId,
                    dc.CommandeRoleNo,
                    dc.CommandeNo
                })
                .HasPrincipalKey(p => new
                {
                    p.UtilisateurId,
                    p.RoleNo,
                    p.No
                })
                .OnDelete(DeleteBehavior.ClientSetNull);

            entité.ToTable("DétailCommandes");
        }
    }
}