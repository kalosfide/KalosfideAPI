using KalosfideAPI.Data.Constantes;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KalosfideAPI.Data
{

    public class Utilisateur
    {
        [Required]
        [MaxLength(LongueurMax.UId)]
        public string Uid { get; set; }

        // données
        [Required]
        public string UserId { get; set; }

        // navigation
        virtual public ApplicationUser ApplicationUser { get; set; }

        virtual public ICollection<Role> Roles { get; set; }
        virtual public ICollection<EtatUtilisateur> Etats { get; set; }

        // utiles
        public string Etat
        {
            get
            {
                int nb = Etats.Count;
                EtatUtilisateur[] etats = new EtatUtilisateur[nb];
                Etats.CopyTo(etats, 0);
                return etats[nb - 1].Etat;
            }
        }

        // création
        public static void CréeTable(ModelBuilder builder)
        {
            var entité = builder.Entity<Utilisateur>();

            entité.HasKey(utilisateur => utilisateur.Uid);

            entité.ToTable("Utilisateurs");
        }
    }

}
