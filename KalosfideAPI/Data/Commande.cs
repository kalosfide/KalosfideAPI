using KalosfideAPI.Roles;
using KalosfideAPI.DétailCommandes;
using KalosfideAPI.Livraisons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KalosfideAPI.Data
{
    public class Commande
    {
        // key
        [Required]
        [MaxLength(LongueurUtilisateurId.Max)]
        public string UtilisateurId { get; set; }
        [Required]
        public int RoleNo { get; set; }
        [Required]
        public int No { get; set; }

        // données
        [Required]
        public DateTime Date { get; set; }
        [MaxLength(LongueurUtilisateurId.Max)]
        public string LivraisonId { get; set; }
        public int? LivraisonNo { get; set; }

        // navigation
        virtual public Role Client { get; set; }
        virtual public Livraison Livraison { get; set; }
        virtual public ICollection<DétailCommande> DétailCommandes { get; set; }

        // création
        public static void CréeTable(ModelBuilder builder)
        {
            var entité = builder.Entity<Commande>();

            entité.HasKey(donnée => new
            {
                donnée.UtilisateurId,
                donnée.RoleNo,
                donnée.No
            });

            entité.HasIndex(donnée => donnée.Date);

            entité
                .HasOne(c => c.Client)
                .WithMany(cl => cl.Commandes)
                .HasForeignKey(c => new
                {
                    c.UtilisateurId,
                    c.RoleNo
                });

            entité.ToTable("Commandes");
        }
    }
}
