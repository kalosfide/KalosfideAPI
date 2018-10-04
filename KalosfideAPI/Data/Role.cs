using KalosfideAPI.Data.Constantes;
using KalosfideAPI.Data.Keys;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KalosfideAPI.Data
{
    public class Role : AKeyUIdRNo
    {
        // key
        [Required]
        [MaxLength(LongueurMax.UtilisateurId)]
        public override string UtilisateurId { get; set; }
        [Required]
        public override int RoleNo { get; set; }

        // données
        [MaxLength(LongueurMax.RoleId)]
        public string AdministrateurId { get; set; }
        [MaxLength(LongueurMax.RoleId)]
        public string FournisseurId { get; set; }
        [MaxLength(LongueurMax.RoleId)]
        public string ClientId { get; set; }

        [StringLength(1)]
        [DefaultValue(EtatRole.Nouveau)]
        public string Etat { get; set; }

        // navigation
        virtual public Utilisateur Utilisateur { get; set; }

        virtual public ICollection<ChangementEtatRole> ChangementsEtat { get; set; }

        virtual public Administrateur Administrateur { get; set; }

        virtual public Fournisseur Fournisseur { get; set; }

        virtual public Client Client { get; set; }

        // utiles
        public string Type
        {
            get
            {
                return ClientId ?? FournisseurId ?? AdministrateurId;
            }
        }
        public void FixeType(string type)
        {
            AdministrateurId = type == TypeDeRole.Administrateur.Code ? RoleId : null;
            FournisseurId = type == TypeDeRole.Fournisseur.Code ? RoleId : null;
            ClientId = type == TypeDeRole.Client.Code ? RoleId : null;
        }

        // création
        public static void CréeTable(ModelBuilder builder)
        {
            var entité = builder.Entity<Role>();

            entité.HasKey(donnée => new
            {
                donnée.UtilisateurId,
                donnée.RoleNo
            });

            entité.HasOne(r => r.Utilisateur).WithMany(u => u.Roles);

            entité.ToTable("Roles");
        }

    }

}