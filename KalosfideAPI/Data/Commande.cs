using KalosfideAPI.Roles;
using KalosfideAPI.DétailCommandes;
using KalosfideAPI.Livraisons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using KalosfideAPI.Data.Constantes;
using KalosfideAPI.Data.Keys;

namespace KalosfideAPI.Data
{
    public class Commande: AKeyUidRnoNo
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

        [MaxLength(LongueurMax.UId)]
        public string LivraisonUid { get; set; }
        public int? LivraisonRno { get; set; }
        public long? LivraisonNo { get; set; }

        // utiles
        public decimal? Prix
        {
            get
            {
                List<DétailCommande> détails= new List<DétailCommande>(DétailCommandes);
                decimal? prix = -1;
                détails.ForEach(d =>
                {
                    if (d.Prix.HasValue)
                    {
                        prix += d.Prix.Value;
                    }
                });
                return prix;
            }
        }

        // navigation
        virtual public Client Client { get; set; }
        virtual public Livraison Livraison { get; set; }
        virtual public ICollection<DétailCommande> DétailCommandes { get; set; }

        // création
        public static void CréeTable(ModelBuilder builder)
        {
            var entité = builder.Entity<Commande>();

            entité.HasKey(donnée => new
            {
                donnée.Uid,
                donnée.Rno,
                donnée.No
            });

            entité
                .HasOne(c => c.Client)
                .WithMany(cl => cl.Commandes)
                .HasForeignKey(c => new { c.Uid, c.Rno });

            entité.ToTable("Commandes");
        }
    }
}
