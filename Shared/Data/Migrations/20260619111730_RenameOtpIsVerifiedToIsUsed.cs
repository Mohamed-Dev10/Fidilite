using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BRICOMA.ECOMMERCE.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameOtpIsVerifiedToIsUsed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsVerified",
                table: "OtpVerifications",
                newName: "IsUsed");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsUsed",
                table: "OtpVerifications",
                newName: "IsVerified");
        }
    }
}
