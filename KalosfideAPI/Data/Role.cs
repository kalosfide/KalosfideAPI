using KalosfideAPI.ChangementsDeRole;
using KalosfideAPI.Commandes;
using KalosfideAPI.Livraisons;
using KalosfideAPI.Partages;
using KalosfideAPI.Produits;
using KalosfideAPI.Utilisateurs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KalosfideAPI.Data
{
    public class Role : IKeyUtilisateurIdNo
    {

        // key
        [Required]
        [MaxLength(LongueurUtilisateurId.Max)]
        public string UtilisateurId { get; set; }
        [Required]
        public int No { get; set; }

        // données
        [Required]
        [StringLength(1)]
        public string Type { get; set; }

        [StringLength(1)]
        [DefaultValue(EtatRole.Nouveau)]
        public string Etat { get; set; }

        [Required]
        [MaxLength(200)]
        public string Nom { get; set; }
        [MaxLength(500)]
        public string Adresse { get; set; }

        [MaxLength(LongueurUtilisateurId.Max)]
        public string FournisseurId { get; set; }
        public long? FournisseurNo { get; set; }

        // navigation
        virtual public Utilisateur Utilisateur { get; set; }

        virtual public ICollection<ChangementEtatRole> ChangementsEtat { get; set; }

        virtual public Role Fournisseur { get; set; }

        [InverseProperty("Fournisseur")]
        virtual public ICollection<Role> Clients { get; set; }

        [InverseProperty("Producteur")]
        virtual public ICollection<Produit> Produits { get; set; }

        [InverseProperty("Livreur")]
        virtual public ICollection<Livraison> Livraisons { get; set; }

        [InverseProperty("Client")]
        virtual public ICollection<Commande> Commandes { get; set; }

        // création
        public static void CréeTable(ModelBuilder builder)
        {
            var entité = builder.Entity<Role>();

            entité.HasKey(donnée => new
            {
                donnée.UtilisateurId,
                donnée.No
            });

            entité.HasOne(r => r.Utilisateur).WithMany(u => u.Roles);

            entité.HasIndex(donnée => donnée.Type);
            entité.HasIndex(donnée => donnée.Nom).IsUnique();
            entité.HasIndex(donnée => donnée.FournisseurId);

            entité.ToTable("Roles");
        }

    }
}