using KalosfideAPI.Data.Constantes;
using KalosfideAPI.Data.Keys;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KalosfideAPI.Data
{
    public class Client : AKeyRId
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

        [MaxLength(LongueurMax.RoleId)]
        public string FournisseurId { get; set; }

        // navigation
        virtual public Role Role { get; set; }

        virtual public Fournisseur Fournisseur { get; set; }

        [InverseProperty("Client")]
        virtual public ICollection<Commande> Commandes { get; set; }

        // utiles

        // création
        public static void CréeTable(ModelBuilder builder)
        {
            var entité = builder.Entity<Client>();

            entité.HasKey(donnée => donnée.RoleId);

            entité.HasOne(client => client.Fournisseur).WithMany(fournisseur => fournisseur.Clients);

            entité
                .HasOne(client => client.Role)
                .WithOne(role => role.Client)
                .HasForeignKey<Client>(client => client.RoleId)
                .HasPrincipalKey<Role>(role => role.ClientId);

            entité.HasIndex(donnée => donnée.Nom);
            entité.HasIndex(donnée => donnée.FournisseurId);

            entité.ToTable("Clients");
        }

    }

}