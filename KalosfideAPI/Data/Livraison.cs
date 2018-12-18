using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KalosfideAPI.Data.Constantes;
using Microsoft.EntityFrameworkCore;

namespace KalosfideAPI.Data
{
    public class Livraison: Keys.AKeyUidRnoNo
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
        public DateTime Date { get; set; }

        // navigation
        virtual public ICollection<Commande> Commandes { get; set; }
        virtual public Fournisseur Livreur { get; set; }

        // création
        public static void CréeTable(ModelBuilder builder)
        {
            var entité = builder.Entity<Livraison>();

            entité.HasKey(donnée => new { donnée.Uid, donnée.Rno, donnée.No });

            entité.HasIndex(donnée => donnée.Date);

            entité
                .HasOne(livraison => livraison.Livreur)
                .WithMany(livreur => livreur.Livraisons)
                .HasForeignKey(livraison => new { livraison.Uid, livraison.Rno });

            entité
                .HasMany(l => l.Commandes)
                .WithOne(c => c.Livraison)
                .HasForeignKey(c => new { c.LivraisonUid, c.LivraisonRno, c.LivraisonNo });

            entité.ToTable("Livraisons");
        }
    }
}
