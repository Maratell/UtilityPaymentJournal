using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UtilityPaymentJournal.Migrations
{
    /// <inheritdoc />
    public partial class AddElectricityTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "electricityreadings",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResidenceId = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    UtilityProviderId = table.Column<long>(type: "bigint", nullable: true),
                    SubmissionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CurrentValue = table.Column<long>(type: "bigint", nullable: false),
                    PreviousValue = table.Column<long>(type: "bigint", nullable: false),
                    ResultValue = table.Column<long>(type: "bigint", nullable: false),
                    PaymentAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_electricityreadings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_electricityreadings_aspnetusers_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "aspnetusers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_electricityreadings_residences_ResidenceId",
                        column: x => x.ResidenceId,
                        principalSchema: "public",
                        principalTable: "residences",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_electricityreadings_utilityproviders_UtilityProviderId",
                        column: x => x.UtilityProviderId,
                        principalSchema: "public",
                        principalTable: "utilityproviders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_electricityreadings_ResidenceId",
                schema: "public",
                table: "electricityreadings",
                column: "ResidenceId");

            migrationBuilder.CreateIndex(
                name: "IX_electricityreadings_UserId",
                schema: "public",
                table: "electricityreadings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_electricityreadings_UtilityProviderId",
                schema: "public",
                table: "electricityreadings",
                column: "UtilityProviderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "electricityreadings",
                schema: "public");
        }
    }
}
