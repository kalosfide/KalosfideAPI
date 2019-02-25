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
            Data.EtatUtilisateur.CréeTable(builder);

            Data.Role.CréeTable(builder);
            Data.EtatRole.CréeTable(builder);

            Data.Site.CréeTable(builder);
            Data.EtatSite.CréeTable(builder);

            Data.Administrateur.CréeTable(builder);
            Data.Fournisseur.CréeTable(builder);
            Data.EtatFournisseur.CréeTable(builder);
            Data.Client.CréeTable(builder);
            Data.EtatClient.CréeTable(builder);

            // Tables de données
            Data.Catégorie.CréeTable(builder);
            Data.Produit.CréeTable(builder);
            Data.Commande.CréeTable(builder);
            Data.Livraison.CréeTable(builder);
            Data.DétailCommande.CréeTable(builder);
            Data.Facture.CréeTable(builder);

            // journaux
            Data.EtatProduit.CréeTable(builder);
            Data.EtatCatégorie.CréeTable(builder);
        }

        public DbSet<Utilisateur> Utilisateur { get; set; }
        public DbSet<EtatUtilisateur> EtatUtilisateur { get; set; }

        public DbSet<Role> Role { get; set; }
        public DbSet<EtatRole> EtatRole { get; set; }

        public DbSet<Site> Site { get; set; }
        public DbSet<EtatSite> EtatSite { get; set; }

        public DbSet<Administrateur> Administrateur { get; set; }
        public DbSet<Fournisseur> Fournisseur { get; set; }
        public DbSet<EtatFournisseur> EtatFournisseur { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<EtatClient> EtatClient { get; set; }

        public DbSet<Catégorie> Catégorie { get; set; }
        public DbSet<EtatCatégorie> EtatCatégorie { get; set; }
        public DbSet<Produit> Produit { get; set; }
        public DbSet<EtatProduit> EtatProduit { get; set; }

        public DbSet<Commande> Commande { get; set; }
        public DbSet<DétailCommande> DétailCommande { get; set; }
        public DbSet<Livraison> Livraison { get; set; }
        public DbSet<Facture> Facture { get; set; }

    }
}
