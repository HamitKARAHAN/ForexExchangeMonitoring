using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ForexExchangeMonitoring.Infrastructure.Data.Migrations
{
    public partial class ForeignKeysChangedMigration : Migration
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
                    FromCurrencyCodeCurrencyModelId = table.Column<int>(type: "int", nullable: true),
                    ToCurrencyCodeCurrencyModelId = table.Column<int>(type: "int", nullable: true),
                    ExchangeRate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastRefreshedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealTimeCurrencyExchangeRates", x => x.ForexCurrencyModelId);
                    table.ForeignKey(
                        name: "FK_RealTimeCurrencyExchangeRates_Currencies_FromCurrencyCodeCurrencyModelId",
                        column: x => x.FromCurrencyCodeCurrencyModelId,
                        principalTable: "Currencies",
                        principalColumn: "CurrencyModelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RealTimeCurrencyExchangeRates_Currencies_ToCurrencyCodeCurrencyModelId",
                        column: x => x.ToCurrencyCodeCurrencyModelId,
                        principalTable: "Currencies",
                        principalColumn: "CurrencyModelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RealTimeCurrencyExchangeRates_FromCurrencyCodeCurrencyModelId",
                table: "RealTimeCurrencyExchangeRates",
                column: "FromCurrencyCodeCurrencyModelId");

            migrationBuilder.CreateIndex(
                name: "IX_RealTimeCurrencyExchangeRates_ToCurrencyCodeCurrencyModelId",
                table: "RealTimeCurrencyExchangeRates",
                column: "ToCurrencyCodeCurrencyModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RealTimeCurrencyExchangeRates");

            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
