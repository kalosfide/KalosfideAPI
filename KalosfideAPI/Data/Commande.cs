using KalosfideAPI.Roles;
using KalosfideAPI.DétailCommandes;
using KalosfideAPI.Livraisons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using KalosfideAPI.Data.Constantes;

namespace KalosfideAPI.Data
{
    public class Commande: Keys.AKeyUidRnoNo
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
        [MaxLength(LongueurMax.RoleId)]
        public string LivraisonRoleId { get; set; }
        public long LivraisonNo { get; set; }

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

            entité.HasIndex(donnée => donnée.Date);

            entité
                .HasOne(c => c.Client)
                .WithMany(cl => cl.Commandes)
                .HasForeignKey(c => new { c.Uid, c.Rno });

            entité.ToTable("Commandes");
        }
    }
}
