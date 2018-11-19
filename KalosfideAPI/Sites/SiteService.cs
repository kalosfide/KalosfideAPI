using KalosfideAPI.Data;
using KalosfideAPI.Partages.KeyParams;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Sites
{
    public class SiteService : KeyUidRnoService<Site>, ISiteService
    {
        public SiteService(ApplicationContext context) : base(context)
        {
            _dbSet = _context.Site;
        }

        public async Task<Site> TrouveParNom(string nomSite)
        {
            return await _dbSet.Where(site => site.NomSite == nomSite).FirstOrDefaultAsync();
        }
    }
}
