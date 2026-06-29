using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UtilityPaymentJournal.Migrations
{
    /// <inheritdoc />
    public partial class AddUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "public",
                table: "waterreadings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "public",
                table: "utilityproviders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "public",
                table: "utilities",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "public",
                table: "residences",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_waterreadings_UserId",
                schema: "public",
                table: "waterreadings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_utilityproviders_UserId",
                schema: "public",
                table: "utilityproviders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_utilities_UserId",
                schema: "public",
                table: "utilities",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_residences_UserId",
                schema: "public",
                table: "residences",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_residences_aspnetusers_UserId",
                schema: "public",
                table: "residences",
                column: "UserId",
                principalSchema: "public",
                principalTable: "aspnetusers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_utilities_aspnetusers_UserId",
                schema: "public",
                table: "utilities",
                column: "UserId",
                principalSchema: "public",
                principalTable: "aspnetusers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_utilityproviders_aspnetusers_UserId",
                schema: "public",
                table: "utilityproviders",
                column: "UserId",
                principalSchema: "public",
                principalTable: "aspnetusers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_waterreadings_aspnetusers_UserId",
                schema: "public",
                table: "waterreadings",
                column: "UserId",
                principalSchema: "public",
                principalTable: "aspnetusers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_residences_aspnetusers_UserId",
                schema: "public",
                table: "residences");

            migrationBuilder.DropForeignKey(
                name: "FK_utilities_aspnetusers_UserId",
                schema: "public",
                table: "utilities");

            migrationBuilder.DropForeignKey(
                name: "FK_utilityproviders_aspnetusers_UserId",
                schema: "public",
                table: "utilityproviders");

            migrationBuilder.DropForeignKey(
                name: "FK_waterreadings_aspnetusers_UserId",
                schema: "public",
                table: "waterreadings");

            migrationBuilder.DropIndex(
                name: "IX_waterreadings_UserId",
                schema: "public",
                table: "waterreadings");

            migrationBuilder.DropIndex(
                name: "IX_utilityproviders_UserId",
                schema: "public",
                table: "utilityproviders");

            migrationBuilder.DropIndex(
                name: "IX_utilities_UserId",
                schema: "public",
                table: "utilities");

            migrationBuilder.DropIndex(
                name: "IX_residences_UserId",
                schema: "public",
                table: "residences");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "public",
                table: "waterreadings");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "public",
                table: "utilityproviders");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "public",
                table: "utilities");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "public",
                table: "residences");
        }
    }
}
