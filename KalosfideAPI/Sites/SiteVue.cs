using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Newtonsoft.Json;

namespace KalosfideAPI.Sites
{
    public class SiteVue : AKeyUidRno
    {
        public override string Uid { get; set; }
        public override int Rno { get; set; }
        public string NomSite { get; set; }
        public string Titre { get; set; }
        public string Etat { get; set; }
        public DateTime? DateEtat { get; set; }
        public int? NbProduits { get; set; }

        [JsonIgnore]
        public bool Ouvert
        {
           get => Etat == Data.Constantes.TypeEtatSite.Actif && DateEtat < DateTime.Now;
        }

    }
}