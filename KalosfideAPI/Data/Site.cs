﻿using KalosfideAPI.Data.Constantes;
using KalosfideAPI.Data.Keys;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [StringLength(1)]
        public string Etat { get; set; }
        public DateTime? DateEtat { get; set; }

        // navigation
        [JsonIgnore]
        virtual public ICollection<Role> Usagers { get; set; }

        // calculés

        // création
        public static void CréeTable(ModelBuilder builder)
        {
            var entité = builder.Entity<Site>();

            entité.HasKey(donnée => new { donnée.Uid, donnée.Rno });

            entité.Property(donnée => donnée.Etat).HasDefaultValue(TypeEtatSite.Nouveau);

            entité.ToTable("Sites");
        }
    }
}
