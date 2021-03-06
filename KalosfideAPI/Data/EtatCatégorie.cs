﻿using KalosfideAPI.Data.Constantes;
using KalosfideAPI.Data.Keys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Data
{
    public class EtatCatégorie: AKeyUidRnoNo
    {
        // key
        [Required]
        [MaxLength(LongueurMax.UId)]
        public override string Uid { get; set; }
        [Required]
        public override int Rno { get; set; }
        [Required]
        public override long No { get; set; }
        [Required]
        public DateTime Date { get; set; }

        // données
        [Required]
        [MinLength(10), MaxLength(200)]
        public string Nom { get; set; }

        // utiles
        static Catégorie Catégorie(EtatCatégorie etatCatégorie)
        {
            return new Catégorie
            {
                Uid = etatCatégorie.Uid,
                Rno = etatCatégorie.Rno,
                No = etatCatégorie.No,
                Nom = etatCatégorie.Nom
            };
        }
        
        // création
        public static void CréeTable(ModelBuilder builder)
        {
            var entité = builder.Entity<EtatCatégorie>();

            entité.HasKey(donnée => new { donnée.Uid, donnée.Rno, donnée.No, donnée.Date });

            entité.HasIndex(donnée => new { donnée.Uid, donnée.Rno, donnée.No });

            entité.ToTable("EtatCatégories");
        }

    }
}
