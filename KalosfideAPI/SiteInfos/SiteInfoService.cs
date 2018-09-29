using KalosfideAPI.Data;
using KalosfideAPI.Erreurs;
using KalosfideAPI.Partages.KeyLong;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.SiteInfos
{
    public class SiteInfoService : KeyLongService<SiteInfo>, ISiteInfoService
    {
        private readonly IConfiguration _configuration;

        public SiteInfoService(
            IConfiguration configuration,
            ApplicationContext context
            ) : base(context, context.SiteInfo)
        {
            _configuration = configuration;
            DValidation = _Validation;
        }

        public async Task<ErreurDeModel> _Validation(SiteInfo donnée)
        {
            if (await EstDoublon(donnée))
            {
                return new ErreurDeModel
                {
                    Code = "Doublon_Nom",
                    Description = $"Le nom {donnée.Nom} est déjà utilisé."
                };
            }
            return null;
        }
        public async Task<bool> EstDoublon(SiteInfo donnée)
        {
            var doublon = await _context.SiteInfo.Where(s => s.Nom == donnée.Nom && s.Id != donnée.Id).FirstOrDefaultAsync();
            return doublon != null;
        }

        public async Task<SiteInfo> Read()
        {
            var manquant = !(await _context.SiteInfo.AnyAsync());
            SiteInfo siteInfo = null;
            if (manquant)
            {
                siteInfo = new SiteInfo
                {
                    Nom = "kalofide.fr",
                    Titre = "Kalosfide",
                    Date = DateTime.Now.Year.ToString()
                };
                // Initialisation Enregistrement des infos sur le site dans la base
                _context.SiteInfo.Add(siteInfo);
                await _context.SaveChangesAsync();
            }
            else
            {
                siteInfo = await _context.SiteInfo.FirstOrDefaultAsync();
            }
            return siteInfo;
        }
    }
}
