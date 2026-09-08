using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UtilityPaymentJournal.Migrations
{
    /// <inheritdoc />
    public partial class AddHeatingUtility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "public",
                table: "utilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 8, 18, 33, 48, 560, DateTimeKind.Utc).AddTicks(2513));

            migrationBuilder.UpdateData(
                schema: "public",
                table: "utilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 8, 18, 33, 48, 560, DateTimeKind.Utc).AddTicks(2515));

            migrationBuilder.InsertData(
                schema: "public",
                table: "utilities",
                columns: new[] { "Id", "CreatedAt", "IconClass", "IsActive", "Name", "UpdatedAt" },
                values: new object[] { 3L, new DateTime(2026, 9, 8, 18, 33, 48, 560, DateTimeKind.Utc).AddTicks(2516), "bi bi-sun-fill text-danger", true, "Отопление", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "public",
                table: "utilities",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "utilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 8, 17, 28, 5, 674, DateTimeKind.Utc).AddTicks(1186));

            migrationBuilder.UpdateData(
                schema: "public",
                table: "utilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 8, 17, 28, 5, 674, DateTimeKind.Utc).AddTicks(1188));
        }
    }
}
