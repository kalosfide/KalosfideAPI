using KalosfideAPI.Data.Constantes;
using KalosfideAPI.Data.Keys;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KalosfideAPI.Data
{
    public class Role : AKeyUidRno
    {
        // key
        [Required]
        [MaxLength(LongueurMax.UId)]
        public override string Uid { get; set; }
        [Required]
        public override int Rno { get; set; }

        [MaxLength(LongueurMax.UId)]
        public string SiteUid { get; set; }
        public int SiteRno { get; set; }

        // navigation
        virtual public Utilisateur Utilisateur { get; set; }

        virtual public ICollection<EtatRole> Etats { get; set; }

        virtual public Site Site { get; set; }

        // utiles
        public KeyParam SiteParam
        {
            get
            {
                return new KeyParam { Uid = SiteUid, Rno = SiteRno };
            }
        }
        public string Etat
        {
            get
            {
                int nb = Etats.Count;
                EtatRole[] etats = new EtatRole[nb];
                Etats.CopyTo(etats, 0);
                return etats[nb - 1].Etat;
            }
        }
        public bool EstAdministrateur { get => SiteUid == null; }
        public bool EstFournisseur { get => SiteUid == Uid && SiteRno == Rno; }
        public bool EstClient { get => !EstAdministrateur && !EstFournisseur; }

        // création
        public static void CréeTable(ModelBuilder builder)
        {
            var entité = builder.Entity<Role>();

            entité.HasKey(donnée => new
            {
                donnée.Uid,
                donnée.Rno
            });

            entité.HasIndex(role => new { role.Uid, role.Rno, });

            entité.HasOne(r => r.Utilisateur).WithMany(u => u.Roles).HasForeignKey(r => r.Uid).HasPrincipalKey(u => u.Uid);

            entité.HasOne(r => r.Site).WithMany(s => s.Usagers).HasForeignKey(r => new { r.SiteUid, r.SiteRno }).HasPrincipalKey(s => new { s.Uid, s.Rno });

            entité.ToTable("Roles");
        }

    }

}