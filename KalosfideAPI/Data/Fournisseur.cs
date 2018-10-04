using KalosfideAPI.Data.Constantes;
using KalosfideAPI.Data.Keys;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KalosfideAPI.Data
{
    public class Fournisseur: AKeyRId
    {
        // key
        [Required]
        [MaxLength(LongueurMax.RoleId)]
        public override string RoleId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Nom { get; set; }
        [MaxLength(500)]
        public string Adresse { get; set; }

        [MaxLength(200)]
        public string NomSite { get; set; }
        [MaxLength(200)]
        public string Titre { get; set; }

        // navigation
        virtual public Role Role { get; set; }

        [InverseProperty("Fournisseur")]
        virtual public ICollection<Client> Clients { get; set; }

        [InverseProperty("Producteur")]
        virtual public ICollection<Produit> Produits { get; set; }

        [InverseProperty("Livreur")]
        virtual public ICollection<Livraison> Livraisons { get; set; }

        // utiles

        // création
        public static void CréeTable(ModelBuilder builder)
        {
            var entité = builder.Entity<Fournisseur>();

            entité.HasKey(donnée => donnée.RoleId);

            entité
                .HasOne(fournisseur => fournisseur.Role)
                .WithOne(role => role.Fournisseur)
                .HasForeignKey<Fournisseur>(fournisseur => fournisseur.RoleId)
                .HasPrincipalKey<Role>(role => role.FournisseurId);

            entité.HasIndex(donnée => donnée.Nom);

            entité.ToTable("Fournisseurs");
        }

    }

}