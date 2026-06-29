using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UtilityPaymentJournal.Migrations
{
    /// <inheritdoc />
    public partial class UpdateComplaintLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_utilities_aspnetusers_UserId",
                schema: "public",
                table: "utilities");

            migrationBuilder.DropForeignKey(
                name: "FK_utilityproviderlink_aspnetusers_UserId",
                schema: "public",
                table: "utilityproviderlink");

            migrationBuilder.DropIndex(
                name: "IX_utilityproviderlink_UserId",
                schema: "public",
                table: "utilityproviderlink");

            migrationBuilder.DropIndex(
                name: "IX_utilities_UserId",
                schema: "public",
                table: "utilities");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "public",
                table: "utilityproviderlink");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "public",
                table: "utilities");

            migrationBuilder.DropColumn(
                name: "UtilityName",
                schema: "public",
                table: "complaints");

            migrationBuilder.AddColumn<long>(
                name: "UtilityId",
                schema: "public",
                table: "complaints",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_complaints_UtilityId",
                schema: "public",
                table: "complaints",
                column: "UtilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_complaints_utilities_UtilityId",
                schema: "public",
                table: "complaints",
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
                name: "FK_complaints_utilities_UtilityId",
                schema: "public",
                table: "complaints");

            migrationBuilder.DropIndex(
                name: "IX_complaints_UtilityId",
                schema: "public",
                table: "complaints");

            migrationBuilder.DropColumn(
                name: "UtilityId",
                schema: "public",
                table: "complaints");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "public",
                table: "utilityproviderlink",
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
                name: "UtilityName",
                schema: "public",
                table: "complaints",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_utilityproviderlink_UserId",
                schema: "public",
                table: "utilityproviderlink",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_utilities_UserId",
                schema: "public",
                table: "utilities",
                column: "UserId");

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
                name: "FK_utilityproviderlink_aspnetusers_UserId",
                schema: "public",
                table: "utilityproviderlink",
                column: "UserId",
                principalSchema: "public",
                principalTable: "aspnetusers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
