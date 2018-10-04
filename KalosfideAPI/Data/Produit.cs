﻿using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KalosfideAPI.Data
{
    public class Produit: Keys.AKeyRIdNo
    {
        // key
        [Required]
        [MaxLength(Constantes.LongueurMax.RoleId)]
        public override string RoleId { get; set; }
        [Required]
        public override long No { get; set; }

        // données
        [Required]
        [MinLength(10), MaxLength(200)]
        public string Nom { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        [Required]
        public decimal Unité { get; set; }
        [DefaultValue(true)]
        public bool QuantitéVautUnités { get; set; }

        // navigation
        virtual public Fournisseur Producteur { get; set; }
        virtual public ICollection<Prix> Prix { get; set; }

        // utiles
        public Prix PrixEnCours
        {
            get
            {
                Prix[] prix = new Prix[0];
                if (Prix != null)
                {
                    Prix.CopyTo(prix, 0);
                }
                return prix.Length > 0 ? prix[prix.Length - 1] : null;
            }
        }
        public bool Indisponible
        {
            get
            {
                return PrixEnCours == null || PrixEnCours.Indisponible;
            }
        }

        // création
        public static void CréeTable(ModelBuilder builder)
        {
            var entité = builder.Entity<Produit>();

            entité.HasKey(donnée => new
            {
                donnée.RoleId,
                donnée.No
            });

            entité.HasIndex(donnée => donnée.Nom).IsUnique();

            entité
                .HasOne(produit => produit.Producteur)
                .WithMany(producteur => producteur.Produits)
                .HasForeignKey(produit => new
                {
                    produit.RoleId,
                    produit.No
                });

            entité.ToTable("Produits");
        }
    }
}
