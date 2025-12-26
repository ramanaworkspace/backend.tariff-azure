using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TariffCalculator.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "calculation_records",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CountryOfOrigin = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HtsCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    ProductCostPerUnit = table.Column<decimal>(type: "numeric", nullable: false),
                    SalePricePerUnit = table.Column<decimal>(type: "numeric", nullable: false),
                    AbsorptionRate = table.Column<decimal>(type: "numeric", nullable: false),
                    TariffRate = table.Column<decimal>(type: "numeric", nullable: false),
                    TariffType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DutyAmountPerUnit = table.Column<decimal>(type: "numeric", nullable: false),
                    MarginPercentPerUnit = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_calculation_records", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "calculation_records");
        }
    }
}
