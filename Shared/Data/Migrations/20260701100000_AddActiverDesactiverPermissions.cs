using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BRICOMA.ECOMMERCE.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddActiverDesactiverPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Category", "Code", "Label" },
                values: new object[,]
                {
                    { 10, "Cartes", "carte.activer",    "Activer une carte" },
                    { 11, "Cartes", "carte.desactiver", "Désactiver une carte" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValues: new object[] { 10, 11 });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Category", "Code", "Label" },
                values: new object[] { 5, "Cartes", "carte.lock", "Bloquer / Débloquer" });
        }
    }
}
