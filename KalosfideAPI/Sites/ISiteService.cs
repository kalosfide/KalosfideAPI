using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Partages.KeyParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Sites
{
    public interface ISiteService : IKeyUidRnoService<Site, SiteVue>
    {
        Task<Site> TrouveParNom(string nomSite);
        Task<bool> NomPris(string nomSite);
        Task<bool> NomPrisParAutre(AKeyUidRno key, string nomSite);
        Task<bool> TitrePris(string titre);
        Task<bool> TitrePrisParAutre(AKeyUidRno key, string titre);
    }
}
