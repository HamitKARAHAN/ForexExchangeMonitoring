using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ForexExchangeMonitoring.Infrastructure.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RealTimeCurrencyExchangeRates",
                columns: table => new
                {
                    ForexCurrencyModelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromCurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToCurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExchangeRate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastRefreshedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealTimeCurrencyExchangeRates", x => x.ForexCurrencyModelId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RealTimeCurrencyExchangeRates");
        }
    }
}
