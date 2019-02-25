using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KalosfideAPI.Data.Constantes;
using KalosfideAPI.Data.Keys;
using Microsoft.EntityFrameworkCore;

namespace KalosfideAPI.Data
{
    public class EtatProduit : AKeyUidRnoNo
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
        [MinLength(10), MaxLength(200)]
        public string Nom { get; set; }
        public long? CatégorieNo { get; set; }

        [StringLength(1)]
        public string TypeMesure { get; set; }
        [StringLength(1)]
        public string TypeCommande { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal? Prix { get; set; }

        // création
        public static void CréeTable(ModelBuilder builder)
        {
            var entité = builder.Entity<EtatProduit>();

            entité.HasKey(donnée => new { donnée.Uid, donnée.Rno, donnée.No, donnée.Date });

            entité.HasIndex(donnée => new { donnée.Uid, donnée.Rno, donnée.No });

            entité.ToTable("EtatProduits");
        }
    }
}
