using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UtilityPaymentJournal.Migrations
{
    /// <inheritdoc />
    public partial class UpdateComplaintEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "IssueResolutionDate",
                schema: "public",
                table: "complaints",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmissionDate",
                schema: "public",
                table: "complaints",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UtilityName",
                schema: "public",
                table: "complaints",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IssueResolutionDate",
                schema: "public",
                table: "complaints");

            migrationBuilder.DropColumn(
                name: "SubmissionDate",
                schema: "public",
                table: "complaints");

            migrationBuilder.DropColumn(
                name: "UtilityName",
                schema: "public",
                table: "complaints");
        }
    }
}
