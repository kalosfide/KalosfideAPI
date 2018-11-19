using KalosfideAPI.Data.Constantes;
using KalosfideAPI.Data.Keys;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KalosfideAPI.Data
{
    public class Client : AKeyUidRno
    {
        // key
        [Required]
        [MaxLength(LongueurMax.UId)]
        public override string Uid { get; set; }
        [Required]
        public override int Rno { get; set; }

        [Required]
        [MaxLength(200)]
        public string Nom { get; set; }
        [MaxLength(500)]
        public string Adresse { get; set; }

        [Required]
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

            entité.HasKey(uidRno =>  new { uidRno.Uid, uidRno.Rno });

            entité.HasOne(client => client.Fournisseur).WithMany(fournisseur => fournisseur.Clients);

            entité
                .HasOne(client => client.Role)
                .WithOne()
                .HasForeignKey<Client>(uidRno =>  new { uidRno.Uid, uidRno.Rno })
                .HasPrincipalKey<Role>(uidRno =>  new { uidRno.Uid, uidRno.Rno }).OnDelete(DeleteBehavior.ClientSetNull);

            entité.HasIndex(donnée => donnée.Nom);
            entité.HasIndex(donnée => donnée.FournisseurId);

            entité.ToTable("Clients");
        }

    }

}