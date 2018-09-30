using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Data
{
    public class Prix : Keys.AKeyUIdRNoNo
    {
        // key
        [Required]
        [MaxLength(LongueurUtilisateurId.Max)]
        public override string UtilisateurId { get; set; }
        [Required]
        public override int RoleNo { get; set; }
        [Required]
        public override long No { get; set; }
        [Required]
        public DateTime Date { get; set; }

        // données
        [Required]
        [DisplayFormat]
        public decimal PrixUnitaire { get; set; }

        // navigation
        virtual public Produit Produit { get; set; }

        // utiles
        public bool Indisponible
        {
            get
            {
                return PrixUnitaire == -1;
            }
        }

        // création
        public static void CréeTable(ModelBuilder builder)
        {
            var entité = builder.Entity<Prix>();

            entité.HasKey(donnée => new
            {
                donnée.UtilisateurId,
                donnée.RoleNo,
                donnée.No,
                donnée.Date
            });

            entité
                .HasOne(prix => prix.Produit)
                .WithMany(produit => produit.Prix)
                .HasForeignKey(prix => new
                {
                    prix.UtilisateurId,
                    prix.RoleNo,
                    prix.No
                });

            entité.ToTable("PrixDesProduits");
        }
    }
}
