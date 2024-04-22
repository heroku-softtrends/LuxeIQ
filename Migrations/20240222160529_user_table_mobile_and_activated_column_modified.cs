using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuxeIQ.Migrations
{
    /// <inheritdoc />
    public partial class user_table_mobile_and_activated_column_modified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "whatsappMobile",
                schema: "public",
                table: "Users",
                newName: "mobile");

            migrationBuilder.RenameColumn(
                name: "activationStatus",
                schema: "public",
                table: "Users",
                newName: "activated");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "mobile",
                schema: "public",
                table: "Users",
                newName: "whatsappMobile");

            migrationBuilder.RenameColumn(
                name: "activated",
                schema: "public",
                table: "Users",
                newName: "activationStatus");
        }
    }
}
