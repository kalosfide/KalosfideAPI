using KalosfideAPI.Data.Constantes;
using KalosfideAPI.Data.Keys;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Data
{
    public class Site : AKeyUidRno
    {
        // key
        [Required]
        [MaxLength(LongueurMax.UId)]
        public override string Uid { get; set; }
        [Required]
        public override int Rno { get; set; }

        // données
        [MaxLength(200)]
        public string NomSite { get; set; }
        [MaxLength(200)]
        public string Titre { get; set; }

        // navigation
        [JsonIgnore]
        virtual public ICollection<Role> Usagers { get; set; }

        // création
        public static void CréeTable(ModelBuilder builder)
        {
            var entité = builder.Entity<Site>();

            entité.HasKey(donnée => new { donnée.Uid, donnée.Rno });

            entité.HasMany(s => s.Usagers).WithOne(r => r.Site).HasForeignKey(r => new { r.SiteUid, r.SiteRno }).HasPrincipalKey(s => new { s.Uid, s.Rno });

            entité.ToTable("Sites");
        }
    }
}
