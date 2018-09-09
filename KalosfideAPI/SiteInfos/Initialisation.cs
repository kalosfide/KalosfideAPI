using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace KalosfideAPI.SiteInfos
{
    public static class Initialisation
    {

        // static method called by the db context on Initialize
        public static void CréeTable(ModelBuilder builder)
        {
            builder.Entity<SiteInfo>().Property(siteInfo => siteInfo.Nom).IsRequired().HasMaxLength(100);

            builder.Entity<SiteInfo>().Property(siteInfo => siteInfo.Titre).HasMaxLength(500);

            builder.Entity<SiteInfo>().Property(siteInfo => siteInfo.Date);

            builder.Entity<SiteInfo>().HasData(new SiteInfo
            {
                Id = 1,
                Nom = "kalofide.fr",
                Titre = "Kalosfide",
                Date = DateTime.Now.Year.ToString()
            });

            builder.Entity<SiteInfo>().ToTable("SiteInfos");
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ISiteInfoService, SiteInfoService>();
            services.AddScoped<ISiteInfoTransformation, SiteInfoTransformation>();
        }
    }
}
