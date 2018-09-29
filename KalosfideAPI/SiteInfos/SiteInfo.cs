using KalosfideAPI.Partages.KeyLong;
using System.ComponentModel.DataAnnotations;

namespace KalosfideAPI.SiteInfos
{
    public class SiteInfo : IKeyLong
    {
        public long Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Nom { get; set; }
        [MaxLength(200)]
        public string Titre { get; set; }
        [MinLength(4), MaxLength(4)]
        public string Date { get; set; }
    }
}
