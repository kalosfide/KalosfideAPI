using KalosfideAPI.Data.Constantes;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace KalosfideAPI.Data
{
    public class EtatSite : Keys.AKeyUidRno
    {
        // key
        [Required]
        [MaxLength(LongueurMax.UId)]
        public override string Uid { get; set; }
        [Required]
        public override int Rno { get; set; }
        [Required]
        public DateTime Date { get; set; }

        // données
        [MaxLength(200)]
        public string NomSite { get; set; }
        [MaxLength(200)]
        public string Titre { get; set; }

        [StringLength(1)]
        public string Etat { get; set; }
        public DateTime? DateEtat { get; set; }

        // création
        public static void CréeTable(ModelBuilder builder)
        {
            var entité = builder.Entity<EtatSite>();

            entité.HasKey(donnée => new
            {
                donnée.Uid,
                donnée.Rno,
                donnée.Date
            });

            entité.ToTable("EtatSite");
        }
    }
}

