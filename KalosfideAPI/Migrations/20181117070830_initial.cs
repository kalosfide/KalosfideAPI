using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KalosfideAPI.Migrations
{
    public partial class initial : Migration
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
                name: "Sites",
                columns: table => new
                {
                    Uid = table.Column<string>(maxLength: 20, nullable: false),
                    Rno = table.Column<int>(nullable: false),
                    NomSite = table.Column<string>(maxLength: 200, nullable: true),
                    Titre = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sites", x => new { x.Uid, x.Rno });
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
                name: "Utilisateurs",
                columns: table => new
                {
                    Uid = table.Column<string>(maxLength: 20, nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilisateurs", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Utilisateurs_ApplicationUser_UserId",
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
                name: "EtatsUtilisateur",
                columns: table => new
                {
                    Uid = table.Column<string>(maxLength: 20, nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Etat = table.Column<string>(maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EtatsUtilisateur", x => new { x.Uid, x.Date });
                    table.ForeignKey(
                        name: "FK_EtatsUtilisateur_Utilisateurs_Uid",
                        column: x => x.Uid,
                        principalTable: "Utilisateurs",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Uid = table.Column<string>(maxLength: 20, nullable: false),
                    Rno = table.Column<int>(nullable: false),
                    SiteUid = table.Column<string>(maxLength: 20, nullable: true),
                    SiteRno = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => new { x.Uid, x.Rno });
                    table.ForeignKey(
                        name: "FK_Roles_Utilisateurs_Uid",
                        column: x => x.Uid,
                        principalTable: "Utilisateurs",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Roles_Sites_SiteUid_SiteRno",
                        columns: x => new { x.SiteUid, x.SiteRno },
                        principalTable: "Sites",
                        principalColumns: new[] { "Uid", "Rno" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Administrateurs",
                columns: table => new
                {
                    Uid = table.Column<string>(maxLength: 20, nullable: false),
                    Rno = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrateurs", x => new { x.Uid, x.Rno });
                    table.ForeignKey(
                        name: "FK_Administrateurs_Roles_Uid_Rno",
                        columns: x => new { x.Uid, x.Rno },
                        principalTable: "Roles",
                        principalColumns: new[] { "Uid", "Rno" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EtatRole",
                columns: table => new
                {
                    Uid = table.Column<string>(maxLength: 20, nullable: false),
                    Rno = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Etat = table.Column<string>(maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EtatRole", x => new { x.Uid, x.Rno, x.Date });
                    table.ForeignKey(
                        name: "FK_EtatRole_Roles_Uid_Rno",
                        columns: x => new { x.Uid, x.Rno },
                        principalTable: "Roles",
                        principalColumns: new[] { "Uid", "Rno" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fournisseurs",
                columns: table => new
                {
                    Uid = table.Column<string>(maxLength: 20, nullable: false),
                    Rno = table.Column<int>(nullable: false),
                    Nom = table.Column<string>(maxLength: 200, nullable: false),
                    Adresse = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fournisseurs", x => new { x.Uid, x.Rno });
                    table.ForeignKey(
                        name: "FK_Fournisseurs_Roles_Uid_Rno",
                        columns: x => new { x.Uid, x.Rno },
                        principalTable: "Roles",
                        principalColumns: new[] { "Uid", "Rno" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Uid = table.Column<string>(maxLength: 20, nullable: false),
                    Rno = table.Column<int>(nullable: false),
                    Nom = table.Column<string>(maxLength: 200, nullable: false),
                    Adresse = table.Column<string>(maxLength: 500, nullable: true),
                    FournisseurId = table.Column<string>(maxLength: 31, nullable: false),
                    FournisseurUid = table.Column<string>(nullable: true),
                    FournisseurRno = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => new { x.Uid, x.Rno });
                    table.ForeignKey(
                        name: "FK_Clients_Fournisseurs_FournisseurUid_FournisseurRno",
                        columns: x => new { x.FournisseurUid, x.FournisseurRno },
                        principalTable: "Fournisseurs",
                        principalColumns: new[] { "Uid", "Rno" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Clients_Roles_Uid_Rno",
                        columns: x => new { x.Uid, x.Rno },
                        principalTable: "Roles",
                        principalColumns: new[] { "Uid", "Rno" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Livraisons",
                columns: table => new
                {
                    Uid = table.Column<string>(maxLength: 20, nullable: false),
                    Rno = table.Column<int>(nullable: false),
                    No = table.Column<long>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livraisons", x => new { x.Uid, x.Rno, x.No });
                    table.ForeignKey(
                        name: "FK_Livraisons_Fournisseurs_Uid_Rno",
                        columns: x => new { x.Uid, x.Rno },
                        principalTable: "Fournisseurs",
                        principalColumns: new[] { "Uid", "Rno" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Produits",
                columns: table => new
                {
                    Uid = table.Column<string>(maxLength: 20, nullable: false),
                    Rno = table.Column<int>(nullable: false),
                    No = table.Column<long>(nullable: false),
                    Nom = table.Column<string>(maxLength: 200, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Unité = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    QuantitéVautUnités = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produits", x => new { x.Uid, x.Rno, x.No });
                    table.ForeignKey(
                        name: "FK_Produits_Fournisseurs_Uid_Rno",
                        columns: x => new { x.Uid, x.Rno },
                        principalTable: "Fournisseurs",
                        principalColumns: new[] { "Uid", "Rno" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Commandes",
                columns: table => new
                {
                    Uid = table.Column<string>(maxLength: 20, nullable: false),
                    Rno = table.Column<int>(nullable: false),
                    No = table.Column<long>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    LivraisonRoleId = table.Column<string>(maxLength: 31, nullable: true),
                    LivraisonNo = table.Column<long>(nullable: false),
                    LivraisonUid = table.Column<string>(nullable: true),
                    LivraisonRno = table.Column<int>(nullable: true),
                    LivraisonNo1 = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commandes", x => new { x.Uid, x.Rno, x.No });
                    table.ForeignKey(
                        name: "FK_Commandes_Clients_Uid_Rno",
                        columns: x => new { x.Uid, x.Rno },
                        principalTable: "Clients",
                        principalColumns: new[] { "Uid", "Rno" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Commandes_Livraisons_LivraisonUid_LivraisonRno_LivraisonNo1",
                        columns: x => new { x.LivraisonUid, x.LivraisonRno, x.LivraisonNo1 },
                        principalTable: "Livraisons",
                        principalColumns: new[] { "Uid", "Rno", "No" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PrixDesProduits",
                columns: table => new
                {
                    Uid = table.Column<string>(maxLength: 20, nullable: false),
                    Rno = table.Column<int>(nullable: false),
                    No = table.Column<long>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    PrixUnitaire = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrixDesProduits", x => new { x.Uid, x.Rno, x.No });
                    table.ForeignKey(
                        name: "FK_PrixDesProduits_Produits_Uid_Rno_No",
                        columns: x => new { x.Uid, x.Rno, x.No },
                        principalTable: "Produits",
                        principalColumns: new[] { "Uid", "Rno", "No" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DétailCommandes",
                columns: table => new
                {
                    CommandeUid = table.Column<string>(maxLength: 20, nullable: false),
                    CommandeRno = table.Column<int>(nullable: false),
                    CommandeNo = table.Column<long>(nullable: false),
                    ProduitUId = table.Column<string>(maxLength: 20, nullable: false),
                    ProduitRno = table.Column<int>(nullable: false),
                    ProduitNo = table.Column<long>(nullable: false),
                    Quantité = table.Column<int>(nullable: false),
                    UnitésLivrées = table.Column<decimal>(type: "decimal(5,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DétailCommandes", x => new { x.CommandeUid, x.CommandeRno, x.CommandeNo, x.ProduitUId, x.ProduitRno, x.ProduitNo });
                    table.ForeignKey(
                        name: "FK_DétailCommandes_Commandes_CommandeUid_CommandeRno_CommandeNo",
                        columns: x => new { x.CommandeUid, x.CommandeRno, x.CommandeNo },
                        principalTable: "Commandes",
                        principalColumns: new[] { "Uid", "Rno", "No" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DétailCommandes_Produits_ProduitUId_ProduitRno_ProduitNo",
                        columns: x => new { x.ProduitUId, x.ProduitRno, x.ProduitNo },
                        principalTable: "Produits",
                        principalColumns: new[] { "Uid", "Rno", "No" },
                        onDelete: ReferentialAction.Restrict);
                });

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
                name: "IX_Clients_FournisseurId",
                table: "Clients",
                column: "FournisseurId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Nom",
                table: "Clients",
                column: "Nom");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_FournisseurUid_FournisseurRno",
                table: "Clients",
                columns: new[] { "FournisseurUid", "FournisseurRno" });

            migrationBuilder.CreateIndex(
                name: "IX_Commandes_Date",
                table: "Commandes",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Commandes_LivraisonUid_LivraisonRno_LivraisonNo1",
                table: "Commandes",
                columns: new[] { "LivraisonUid", "LivraisonRno", "LivraisonNo1" });

            migrationBuilder.CreateIndex(
                name: "IX_DétailCommandes_ProduitUId_ProduitRno_ProduitNo",
                table: "DétailCommandes",
                columns: new[] { "ProduitUId", "ProduitRno", "ProduitNo" });

            migrationBuilder.CreateIndex(
                name: "IX_Fournisseurs_Nom",
                table: "Fournisseurs",
                column: "Nom");

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
                name: "IX_Roles_SiteUid_SiteRno",
                table: "Roles",
                columns: new[] { "SiteUid", "SiteRno" });

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Uid_Rno",
                table: "Roles",
                columns: new[] { "Uid", "Rno" });

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateurs_UserId",
                table: "Utilisateurs",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Administrateurs");

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
                name: "EtatRole");

            migrationBuilder.DropTable(
                name: "EtatsUtilisateur");

            migrationBuilder.DropTable(
                name: "PrixDesProduits");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Commandes");

            migrationBuilder.DropTable(
                name: "Produits");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Livraisons");

            migrationBuilder.DropTable(
                name: "Fournisseurs");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Utilisateurs");

            migrationBuilder.DropTable(
                name: "Sites");

            migrationBuilder.DropTable(
                name: "ApplicationUser");
        }
    }
}
