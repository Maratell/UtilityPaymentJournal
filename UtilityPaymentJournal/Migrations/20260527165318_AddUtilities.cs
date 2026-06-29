using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UtilityPaymentJournal.Migrations
{
    /// <inheritdoc />
    public partial class AddUtilities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_utilityproviderlink_utility_UtilityId",
                schema: "public",
                table: "utilityproviderlink");

            migrationBuilder.DropPrimaryKey(
                name: "PK_utility",
                schema: "public",
                table: "utility");

            migrationBuilder.RenameTable(
                name: "utility",
                schema: "public",
                newName: "utilities",
                newSchema: "public");

            migrationBuilder.AddPrimaryKey(
                name: "PK_utilities",
                schema: "public",
                table: "utilities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_utilityproviderlink_utilities_UtilityId",
                schema: "public",
                table: "utilityproviderlink",
                column: "UtilityId",
                principalSchema: "public",
                principalTable: "utilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_utilityproviderlink_utilities_UtilityId",
                schema: "public",
                table: "utilityproviderlink");

            migrationBuilder.DropPrimaryKey(
                name: "PK_utilities",
                schema: "public",
                table: "utilities");

            migrationBuilder.RenameTable(
                name: "utilities",
                schema: "public",
                newName: "utility",
                newSchema: "public");

            migrationBuilder.AddPrimaryKey(
                name: "PK_utility",
                schema: "public",
                table: "utility",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_utilityproviderlink_utility_UtilityId",
                schema: "public",
                table: "utilityproviderlink",
                column: "UtilityId",
                principalSchema: "public",
                principalTable: "utility",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
