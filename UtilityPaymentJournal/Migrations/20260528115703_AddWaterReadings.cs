using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UtilityPaymentJournal.Migrations
{
    /// <inheritdoc />
    public partial class AddWaterReadings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "waterreadings",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResidenceId = table.Column<long>(type: "bigint", nullable: true),
                    UtilityProviderId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_waterreadings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_waterreadings_residences_ResidenceId",
                        column: x => x.ResidenceId,
                        principalSchema: "public",
                        principalTable: "residences",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_waterreadings_utilityprovider_UtilityProviderId",
                        column: x => x.UtilityProviderId,
                        principalSchema: "public",
                        principalTable: "utilityprovider",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_waterreadings_ResidenceId",
                schema: "public",
                table: "waterreadings",
                column: "ResidenceId");

            migrationBuilder.CreateIndex(
                name: "IX_waterreadings_UtilityProviderId",
                schema: "public",
                table: "waterreadings",
                column: "UtilityProviderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "waterreadings",
                schema: "public");
        }
    }
}
