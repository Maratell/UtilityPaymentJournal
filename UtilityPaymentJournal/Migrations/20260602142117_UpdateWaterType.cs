using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UtilityPaymentJournal.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWaterType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHotWater",
                schema: "public",
                table: "waterreadings");

            migrationBuilder.AddColumn<int>(
                name: "WaterType",
                schema: "public",
                table: "waterreadings",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WaterType",
                schema: "public",
                table: "waterreadings");

            migrationBuilder.AddColumn<bool>(
                name: "IsHotWater",
                schema: "public",
                table: "waterreadings",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
