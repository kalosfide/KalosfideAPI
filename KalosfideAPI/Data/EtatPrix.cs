using KalosfideAPI.Data.Constantes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Data
{
    public class EtatPrix : Keys.AKeyUidRnoNo
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
        public DateTime Date { get; set; }

        // données
        [Required]
        [Column(TypeName ="decimal(5,2)")]
        public decimal Prix { get; set; }

        // navigation
        virtual public Produit Produit { get; set; }

        // création
        public static void CréeTable(ModelBuilder builder)
        {
            var entité = builder.Entity<EtatPrix>();

            entité.HasKey(donnée => new { donnée.Uid, donnée.Rno, donnée.No, donnée.Date });

            entité
                .HasOne(etatPrix => etatPrix.Produit)
                .WithMany(produit => produit.EtatPrix)
                .HasForeignKey(donnée => new { donnée.Uid, donnée.Rno, donnée.No });

            entité.ToTable("EtatPrix");
        }
    }
}
