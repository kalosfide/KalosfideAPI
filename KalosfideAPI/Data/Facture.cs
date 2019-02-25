using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KalosfideAPI.Data.Constantes;
using Microsoft.EntityFrameworkCore;

namespace KalosfideAPI.Data
{
    public class Facture : Keys.AKeyUidRnoNo
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
        public DateTime? Date { get; set; }

        // navigation
        virtual public ICollection<Livraison> Livraisons { get; set; }
        virtual public Fournisseur Vendeur { get; set; }

        // création
        public static void CréeTable(ModelBuilder builder)
        {
            var entité = builder.Entity<Facture>();

            entité.HasKey(donnée => new { donnée.Uid, donnée.Rno, donnée.No });

            entité.HasIndex(donnée => donnée.Date);

            entité
                .HasOne(facture => facture.Vendeur)
                .WithMany(fournisseur => fournisseur.Factures)
                .HasForeignKey(facture => new { facture.Uid, facture.Rno });

            entité
                .HasMany(facture => facture.Livraisons)
                .WithOne(livraison => livraison.Facture)
                .HasForeignKey(livraison => new { livraison.Uid, livraison.Rno, livraison.FactureNo });

            entité.ToTable("Factures");
        }
    }
}
