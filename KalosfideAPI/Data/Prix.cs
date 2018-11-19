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
    public class Prix : Keys.AKeyUidRnoNo
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

            entité.HasKey(donnée => new { donnée.Uid, donnée.Rno, donnée.No });

            entité
                .HasOne(prix => prix.Produit)
                .WithMany(produit => produit.Prix)
                .HasForeignKey(donnée => new { donnée.Uid, donnée.Rno, donnée.No });

            entité.ToTable("PrixDesProduits");
        }
    }
}
