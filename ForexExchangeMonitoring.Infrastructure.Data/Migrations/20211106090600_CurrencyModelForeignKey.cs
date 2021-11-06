using Microsoft.EntityFrameworkCore.Migrations;

namespace ForexExchangeMonitoring.Infrastructure.Data.Migrations
{
    public partial class CurrencyModelForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrencyModel",
                table: "RealTimeCurrencyExchangeRates",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RealTimeCurrencyExchangeRates_CurrencyModel",
                table: "RealTimeCurrencyExchangeRates",
                column: "CurrencyModel");

            migrationBuilder.AddForeignKey(
                name: "FK_RealTimeCurrencyExchangeRates_Currencies_CurrencyModel",
                table: "RealTimeCurrencyExchangeRates",
                column: "CurrencyModel",
                principalTable: "Currencies",
                principalColumn: "CurrencyModelId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RealTimeCurrencyExchangeRates_Currencies_CurrencyModel",
                table: "RealTimeCurrencyExchangeRates");

            migrationBuilder.DropIndex(
                name: "IX_RealTimeCurrencyExchangeRates_CurrencyModel",
                table: "RealTimeCurrencyExchangeRates");

            migrationBuilder.DropColumn(
                name: "CurrencyModel",
                table: "RealTimeCurrencyExchangeRates");
        }
    }
}
