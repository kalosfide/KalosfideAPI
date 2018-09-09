using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.SiteInfos
{
    public class SiteInfoTransformation : ISiteInfoTransformation
    {
        public SiteInfoVue CréeVue(SiteInfo siteInfo)
        {
            return new SiteInfoVue
            {
                Id = siteInfo.Id,
                Nom = siteInfo.Nom,
                Titre = siteInfo.Titre,
                Date = siteInfo.Date
            };
        }

        public IEnumerable<SiteInfoVue> CréeVues(IEnumerable<SiteInfo> siteInfos)
        {
            List<SiteInfoVue> vues = new List<SiteInfoVue>();
            foreach (SiteInfo siteInfo in siteInfos)
            {
                vues.Add(CréeVue(siteInfo));
            }
            return vues;
        }

        public SiteInfo CréeDonnée(SiteInfoVue vue)
        {
            return new SiteInfo
            {
                Id = vue.Id,
                Nom = vue.Nom,
                Titre = vue.Titre,
                Date = vue.Date
            };
        }

        public void CopieVueDansDonnées(SiteInfo donnée, SiteInfoVue vue)
        {
            donnée.Nom = vue.Nom;
            donnée.Titre = vue.Titre;
            donnée.Date = vue.Date;
        }
    }
}
