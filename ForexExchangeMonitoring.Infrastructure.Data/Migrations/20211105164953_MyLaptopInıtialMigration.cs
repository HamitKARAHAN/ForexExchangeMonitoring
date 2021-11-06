using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ForexExchangeMonitoring.Infrastructure.Data.Migrations
{
    public partial class MyLaptopInıtialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    CurrencyModelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.CurrencyModelId);
                });

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
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "RealTimeCurrencyExchangeRates");
        }
    }
}
