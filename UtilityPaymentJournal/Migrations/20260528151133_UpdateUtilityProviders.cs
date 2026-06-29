using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UtilityPaymentJournal.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUtilityProviders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_utilityproviderlink_utilityprovider_UtilityProviderId",
                schema: "public",
                table: "utilityproviderlink");

            migrationBuilder.DropForeignKey(
                name: "FK_waterreadings_utilityprovider_UtilityProviderId",
                schema: "public",
                table: "waterreadings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_utilityprovider",
                schema: "public",
                table: "utilityprovider");

            migrationBuilder.RenameTable(
                name: "utilityprovider",
                schema: "public",
                newName: "utilityproviders",
                newSchema: "public");

            migrationBuilder.AddPrimaryKey(
                name: "PK_utilityproviders",
                schema: "public",
                table: "utilityproviders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_utilityproviderlink_utilityproviders_UtilityProviderId",
                schema: "public",
                table: "utilityproviderlink",
                column: "UtilityProviderId",
                principalSchema: "public",
                principalTable: "utilityproviders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_waterreadings_utilityproviders_UtilityProviderId",
                schema: "public",
                table: "waterreadings",
                column: "UtilityProviderId",
                principalSchema: "public",
                principalTable: "utilityproviders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_utilityproviderlink_utilityproviders_UtilityProviderId",
                schema: "public",
                table: "utilityproviderlink");

            migrationBuilder.DropForeignKey(
                name: "FK_waterreadings_utilityproviders_UtilityProviderId",
                schema: "public",
                table: "waterreadings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_utilityproviders",
                schema: "public",
                table: "utilityproviders");

            migrationBuilder.RenameTable(
                name: "utilityproviders",
                schema: "public",
                newName: "utilityprovider",
                newSchema: "public");

            migrationBuilder.AddPrimaryKey(
                name: "PK_utilityprovider",
                schema: "public",
                table: "utilityprovider",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_utilityproviderlink_utilityprovider_UtilityProviderId",
                schema: "public",
                table: "utilityproviderlink",
                column: "UtilityProviderId",
                principalSchema: "public",
                principalTable: "utilityprovider",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_waterreadings_utilityprovider_UtilityProviderId",
                schema: "public",
                table: "waterreadings",
                column: "UtilityProviderId",
                principalSchema: "public",
                principalTable: "utilityprovider",
                principalColumn: "Id");
        }
    }
}
