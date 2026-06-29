using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UtilityPaymentJournal.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullName",
                schema: "public",
                table: "aspnetusers",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "public",
                table: "aspnetusers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "public",
                table: "aspnetusers");

            migrationBuilder.RenameColumn(
                name: "LastName",
                schema: "public",
                table: "aspnetusers",
                newName: "FullName");
        }
    }
}
