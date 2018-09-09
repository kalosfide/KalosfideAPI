using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KalosfideAPI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiteInfos",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nom = table.Column<string>(maxLength: 100, nullable: false),
                    Titre = table.Column<string>(maxLength: 500, nullable: true),
                    Date = table.Column<string>(maxLength: 4, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Utilisateurs",
                columns: table => new
                {
                    UtilisateurId = table.Column<string>(maxLength: 20, nullable: false),
                    Etat = table.Column<string>(maxLength: 1, nullable: true),
                    UserId = table.Column<string>(nullable: false),
                    RoleSélectionnéNo = table.Column<int>(nullable: false),
                    RoleSélectionnéUtilisateurId = table.Column<string>(nullable: true),
                    RoleSélectionnéNo1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilisateurs", x => x.UtilisateurId);
                    table.ForeignKey(
                        name: "FK_Utilisateurs_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JournalEtatUtilisateur",
                columns: table => new
                {
                    UtilisateurId = table.Column<string>(maxLength: 20, nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Etat = table.Column<string>(maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEtatUtilisateur", x => new { x.UtilisateurId, x.Date });
                    table.ForeignKey(
                        name: "FK_JournalEtatUtilisateur_Utilisateurs_UtilisateurId",
                        column: x => x.UtilisateurId,
                        principalTable: "Utilisateurs",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    UtilisateurId = table.Column<string>(maxLength: 20, nullable: false),
                    No = table.Column<int>(nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    Etat = table.Column<string>(maxLength: 1, nullable: true),
                    Nom = table.Column<string>(maxLength: 200, nullable: false),
                    Adresse = table.Column<string>(maxLength: 500, nullable: true),
                    FournisseurId = table.Column<string>(maxLength: 20, nullable: true),
                    FournisseurNo = table.Column<long>(nullable: true),
                    FournisseurUtilisateurId = table.Column<string>(nullable: true),
                    FournisseurNo1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => new { x.UtilisateurId, x.No });
                    table.ForeignKey(
                        name: "FK_Roles_Utilisateurs_UtilisateurId",
                        column: x => x.UtilisateurId,
                        principalTable: "Utilisateurs",
                        principalColumn: "UtilisateurId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Roles_Roles_FournisseurUtilisateurId_FournisseurNo1",
                        columns: x => new { x.FournisseurUtilisateurId, x.FournisseurNo1 },
                        principalTable: "Roles",
                        principalColumns: new[] { "UtilisateurId", "No" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JournalEtatRole",
                columns: table => new
                {
                    UtilisateurId = table.Column<string>(maxLength: 20, nullable: false),
                    RoleNo = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Etat = table.Column<string>(maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEtatRole", x => new { x.UtilisateurId, x.RoleNo, x.Date });
                    table.ForeignKey(
                        name: "FK_JournalEtatRole_Roles_UtilisateurId_RoleNo",
                        columns: x => new { x.UtilisateurId, x.RoleNo },
                        principalTable: "Roles",
                        principalColumns: new[] { "UtilisateurId", "No" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Livraisons",
                columns: table => new
                {
                    UtilisateurId = table.Column<string>(maxLength: 20, nullable: false),
                    RoleNo = table.Column<int>(nullable: false),
                    No = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livraisons", x => new { x.UtilisateurId, x.RoleNo, x.No });
                    table.ForeignKey(
                        name: "FK_Livraisons_Roles_UtilisateurId_RoleNo",
                        columns: x => new { x.UtilisateurId, x.RoleNo },
                        principalTable: "Roles",
                        principalColumns: new[] { "UtilisateurId", "No" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Produits",
                columns: table => new
                {
                    UtilisateurId = table.Column<string>(maxLength: 20, nullable: false),
                    RoleNo = table.Column<int>(nullable: false),
                    No = table.Column<int>(nullable: false),
                    Nom = table.Column<string>(maxLength: 200, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Indisponible = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produits", x => new { x.UtilisateurId, x.RoleNo, x.No });
                    table.ForeignKey(
                        name: "FK_Produits_Roles_UtilisateurId_RoleNo",
                        columns: x => new { x.UtilisateurId, x.RoleNo },
                        principalTable: "Roles",
                        principalColumns: new[] { "UtilisateurId", "No" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Commandes",
                columns: table => new
                {
                    UtilisateurId = table.Column<string>(maxLength: 20, nullable: false),
                    RoleNo = table.Column<int>(nullable: false),
                    No = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    LivraisonId = table.Column<string>(maxLength: 20, nullable: true),
                    LivraisonNo = table.Column<int>(nullable: true),
                    LivraisonUtilisateurId = table.Column<string>(nullable: true),
                    LivraisonRoleNo = table.Column<int>(nullable: true),
                    LivraisonNo1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commandes", x => new { x.UtilisateurId, x.RoleNo, x.No });
                    table.ForeignKey(
                        name: "FK_Commandes_Roles_UtilisateurId_RoleNo",
                        columns: x => new { x.UtilisateurId, x.RoleNo },
                        principalTable: "Roles",
                        principalColumns: new[] { "UtilisateurId", "No" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Commandes_Livraisons_LivraisonUtilisateurId_LivraisonRoleNo_LivraisonNo1",
                        columns: x => new { x.LivraisonUtilisateurId, x.LivraisonRoleNo, x.LivraisonNo1 },
                        principalTable: "Livraisons",
                        principalColumns: new[] { "UtilisateurId", "RoleNo", "No" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DétailCommandes",
                columns: table => new
                {
                    CommandeUtilisateurId = table.Column<string>(maxLength: 20, nullable: false),
                    CommandeRoleNo = table.Column<int>(nullable: false),
                    CommandeNo = table.Column<int>(nullable: false),
                    ProduitUtilisateurId = table.Column<string>(maxLength: 20, nullable: false),
                    ProduitRoleNo = table.Column<int>(nullable: false),
                    ProduitNo = table.Column<int>(nullable: false),
                    Quantité = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DétailCommandes", x => new { x.CommandeUtilisateurId, x.CommandeRoleNo, x.CommandeNo, x.ProduitUtilisateurId, x.ProduitRoleNo, x.ProduitNo });
                    table.ForeignKey(
                        name: "FK_DétailCommandes_Commandes_CommandeUtilisateurId_CommandeRoleNo_CommandeNo",
                        columns: x => new { x.CommandeUtilisateurId, x.CommandeRoleNo, x.CommandeNo },
                        principalTable: "Commandes",
                        principalColumns: new[] { "UtilisateurId", "RoleNo", "No" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DétailCommandes_Produits_CommandeUtilisateurId_CommandeRoleNo_CommandeNo",
                        columns: x => new { x.CommandeUtilisateurId, x.CommandeRoleNo, x.CommandeNo },
                        principalTable: "Produits",
                        principalColumns: new[] { "UtilisateurId", "RoleNo", "No" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "SiteInfos",
                columns: new[] { "Id", "Date", "Nom", "Titre" },
                values: new object[] { 1L, "2018", "kalofide.fr", "Kalosfide" });

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "ApplicationUser",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "ApplicationUser",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Commandes_Date",
                table: "Commandes",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Commandes_LivraisonUtilisateurId_LivraisonRoleNo_LivraisonNo1",
                table: "Commandes",
                columns: new[] { "LivraisonUtilisateurId", "LivraisonRoleNo", "LivraisonNo1" });

            migrationBuilder.CreateIndex(
                name: "IX_Livraisons_Date",
                table: "Livraisons",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Produits_Nom",
                table: "Produits",
                column: "Nom",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_FournisseurId",
                table: "Roles",
                column: "FournisseurId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Nom",
                table: "Roles",
                column: "Nom",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Type",
                table: "Roles",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_FournisseurUtilisateurId_FournisseurNo1",
                table: "Roles",
                columns: new[] { "FournisseurUtilisateurId", "FournisseurNo1" });

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateurs_UserId",
                table: "Utilisateurs",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateurs_RoleSélectionnéUtilisateurId_RoleSélectionnéNo1",
                table: "Utilisateurs",
                columns: new[] { "RoleSélectionnéUtilisateurId", "RoleSélectionnéNo1" });

            migrationBuilder.AddForeignKey(
                name: "FK_Utilisateurs_Roles_RoleSélectionnéUtilisateurId_RoleSélectionnéNo1",
                table: "Utilisateurs",
                columns: new[] { "RoleSélectionnéUtilisateurId", "RoleSélectionnéNo1" },
                principalTable: "Roles",
                principalColumns: new[] { "UtilisateurId", "No" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utilisateurs_ApplicationUser_UserId",
                table: "Utilisateurs");

            migrationBuilder.DropForeignKey(
                name: "FK_Utilisateurs_Roles_RoleSélectionnéUtilisateurId_RoleSélectionnéNo1",
                table: "Utilisateurs");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "DétailCommandes");

            migrationBuilder.DropTable(
                name: "JournalEtatRole");

            migrationBuilder.DropTable(
                name: "JournalEtatUtilisateur");

            migrationBuilder.DropTable(
                name: "SiteInfos");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Commandes");

            migrationBuilder.DropTable(
                name: "Produits");

            migrationBuilder.DropTable(
                name: "Livraisons");

            migrationBuilder.DropTable(
                name: "ApplicationUser");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Utilisateurs");
        }
    }
}
