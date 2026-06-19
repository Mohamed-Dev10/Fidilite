using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814

namespace BRICOMA.ECOMMERCE.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPermissionsAndRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Ajouter les colonnes custom de ApplicationRole sur la table existante AspNetRoles
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AspNetRoles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetRoles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 1, 1));

            // Créer la table Permissions
            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            // Créer la table RolePermissions
            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Insérer les permissions de base
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Category", "Code", "Label" },
                values: new object[,]
                {
                    { 1, "Cartes",         "carte.create",      "Créer une carte" },
                    { 2, "Cartes",         "carte.list",        "Voir la liste" },
                    { 3, "Cartes",         "carte.confirm",     "Confirmer OTP" },
                    { 4, "Cartes",         "carte.edit",        "Modifier une carte" },
                    { 5, "Cartes",         "carte.lock",        "Bloquer / Débloquer" },
                    { 6, "Paramétrage",    "parametrage.view",  "Voir paramétrage" },
                    { 7, "Paramétrage",    "parametrage.edit",  "Modifier paramétrage" },
                    { 8, "Administration", "admin.users",       "Gérer utilisateurs" },
                    { 9, "Administration", "admin.roles",       "Gérer rôles et permissions" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "RolePermissions");
            migrationBuilder.DropTable(name: "Permissions");

            migrationBuilder.DropColumn(name: "Description", table: "AspNetRoles");
            migrationBuilder.DropColumn(name: "CreatedAt",   table: "AspNetRoles");
        }
    }
}
