using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KalosfideAPI.Data.Keys;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace KalosfideAPI.Sites
{
    public class SiteVue : AKeyUidRno
    {
        public override string Uid { get; set; }
        public override int Rno { get; set; }
        public string NomSite { get; set; }
        public string Titre { get; set; }
    }
}