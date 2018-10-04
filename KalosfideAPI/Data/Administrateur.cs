using KalosfideAPI.Data.Constantes;
using KalosfideAPI.Data.Keys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Data
{
    public class Administrateur : AKeyRId
    {
        // key
        [Required]
        [MaxLength(LongueurMax.RoleId)]
        public override string RoleId { get; set; }

        // navigation
        virtual public Role Role { get; set; }

        // utiles

        // création
        public static void CréeTable(ModelBuilder builder)
        {
            var entité = builder.Entity<Administrateur>();

            entité.HasKey(donnée => donnée.RoleId);

            entité
                .HasOne(administrateur => administrateur.Role)
                .WithOne(role => role.Administrateur)
                .HasForeignKey<Administrateur>(administrateur => administrateur.RoleId)
                .HasPrincipalKey<Role>(role => role.AdministrateurId);

            entité.ToTable("Administrateurs");
        }

    }

}