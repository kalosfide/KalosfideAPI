using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KalosfideAPI.Data
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(DbContextOptions options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            Data.ApplicationUser.CréeTable(builder);

            Data.Utilisateur.CréeTable(builder);
            Data.ChangementEtatUtilisateur.CréeTable(builder);

            Data.Role.CréeTable(builder);
            Data.ChangementEtatRole.CréeTable(builder);

            Data.Administrateur.CréeTable(builder);
            Data.Fournisseur.CréeTable(builder);
            Data.Client.CréeTable(builder);

            // Tables de données
            SiteInfos.Initialisation.CréeTable(builder);
            Data.Commande.CréeTable(builder);
            Data.Livraison.CréeTable(builder);
            Data.Produit.CréeTable(builder);
            Data.Prix.CréeTable(builder);
            Data.DétailCommande.CréeTable(builder);
        }

        public DbSet<Utilisateur> Utilisateur { get; set; }
        public DbSet<ChangementEtatUtilisateur> JournalEtatUtilisateur { get; set; }

        public DbSet<Role> Role { get; set; }
        public DbSet<ChangementEtatRole> JournalEtatRole { get; set; }

        public DbSet<Administrateur> Administrateur { get; set; }
        public DbSet<Fournisseur> Fournisseur { get; set; }
        public DbSet<Client> Client { get; set; }

        public DbSet<SiteInfos.SiteInfo> SiteInfo { get; set; }
        public DbSet<Commande> Commande { get; set; }
        public DbSet<DétailCommande> DétailCommande { get; set; }
        public DbSet<Livraison> Livraison { get; set; }
        public DbSet<Produit> Produit { get; set; }
        public DbSet<Prix> Prix{ get; set; }

    }
}
