using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UtilityPaymentJournal.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserIdFilters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "public",
                table: "utilityproviderlink",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_utilityproviderlink_UserId",
                schema: "public",
                table: "utilityproviderlink",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_utilityproviderlink_aspnetusers_UserId",
                schema: "public",
                table: "utilityproviderlink",
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
                name: "FK_utilityproviderlink_aspnetusers_UserId",
                schema: "public",
                table: "utilityproviderlink");

            migrationBuilder.DropIndex(
                name: "IX_utilityproviderlink_UserId",
                schema: "public",
                table: "utilityproviderlink");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "public",
                table: "utilityproviderlink");
        }
    }
}
