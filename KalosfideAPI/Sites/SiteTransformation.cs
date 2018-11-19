using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KalosfideAPI.Data;

namespace KalosfideAPI.Sites
{
    public class SiteTransformation : ISiteTransformation
    {
        public void CopieVueDansDonnées(Site donnée, SiteVue vue)
        {
            donnée.NomSite = vue.NomSite;
            donnée.Titre = vue.Titre;
        }

        public Site CréeDonnée(SiteVue vue)
        {
            return new Site
            {
                Uid = vue.Uid,
                Rno = vue.Rno,
                NomSite = vue.NomSite,
                Titre = vue.Titre
            };
        }

        public SiteVue CréeVue(Site donnée)
        {
            return new SiteVue
            {
                Uid = donnée.Uid,
                Rno = donnée.Rno,
                NomSite = donnée.NomSite,
                Titre = donnée.Titre
            };
        }

        public IEnumerable<SiteVue> CréeVues(IEnumerable<Site> données)
        {
            List<SiteVue> vues = new List<SiteVue>();
            foreach (Site donnée in données)
            {
                vues.Add(CréeVue(donnée));
            }
            return vues;
        }
    }
}
