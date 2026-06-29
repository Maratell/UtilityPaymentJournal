using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UtilityPaymentJournal.Migrations
{
    /// <inheritdoc />
    public partial class AddUtility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "utility",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_utility", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "utilityprovider",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_utilityprovider", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "utilityproviderlink",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UtilityProviderId = table.Column<long>(type: "bigint", nullable: false),
                    UtilityId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_utilityproviderlink", x => x.Id);
                    table.ForeignKey(
                        name: "FK_utilityproviderlink_utility_UtilityId",
                        column: x => x.UtilityId,
                        principalSchema: "public",
                        principalTable: "utility",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_utilityproviderlink_utilityprovider_UtilityProviderId",
                        column: x => x.UtilityProviderId,
                        principalSchema: "public",
                        principalTable: "utilityprovider",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_utilityproviderlink_UtilityId",
                schema: "public",
                table: "utilityproviderlink",
                column: "UtilityId");

            migrationBuilder.CreateIndex(
                name: "IX_utilityproviderlink_UtilityProviderId_UtilityId",
                schema: "public",
                table: "utilityproviderlink",
                columns: new[] { "UtilityProviderId", "UtilityId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "utilityproviderlink",
                schema: "public");

            migrationBuilder.DropTable(
                name: "utility",
                schema: "public");

            migrationBuilder.DropTable(
                name: "utilityprovider",
                schema: "public");
        }
    }
}
