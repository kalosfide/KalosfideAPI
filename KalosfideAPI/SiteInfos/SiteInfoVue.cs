using KalosfideAPI.Partages;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace KalosfideAPI.SiteInfos
{
    public class SiteInfoVue : IKeyLong
    {
        public long Id { get; set; }
        public string Nom { get; set; }
        public string Titre { get; set; }
        public string Date { get; set; }
    }
}
