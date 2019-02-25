using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Enregistrement;
using KalosfideAPI.Partages;
using KalosfideAPI.Partages.KeyParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Sites
{
    public interface ISiteService : IKeyUidRnoService<Site, SiteVue>
    {
        Site CréeSite(Role role, EnregistrementFournisseurVue fournisseurVue);

        Task<EtatSite> Etat(AKeyUidRno key);
        Task<bool> Ouvert(AKeyUidRno key);
        Task<RetourDeService> Ouvre(KeyUidRno key);
        Task<RetourDeService> Ferme(KeyUidRno key, DateTime jusquA);
        Task<SiteVue> TrouveParNom(string nomSite);
        Task<bool> NomPris(string nomSite);
        Task<bool> NomPrisParAutre(AKeyUidRno key, string nomSite);
        Task<bool> TitrePris(string titre);
        Task<bool> TitrePrisParAutre(AKeyUidRno key, string titre);
    }
}
