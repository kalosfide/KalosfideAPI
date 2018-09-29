using KalosfideAPI.Partages.KeyLong;

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
