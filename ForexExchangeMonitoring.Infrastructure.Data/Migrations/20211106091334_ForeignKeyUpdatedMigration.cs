using Microsoft.EntityFrameworkCore.Migrations;

namespace ForexExchangeMonitoring.Infrastructure.Data.Migrations
{
    public partial class ForeignKeyUpdatedMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RealTimeCurrencyExchangeRates_Currencies_CurrencyModel",
                table: "RealTimeCurrencyExchangeRates");

            migrationBuilder.RenameColumn(
                name: "CurrencyModel",
                table: "RealTimeCurrencyExchangeRates",
                newName: "CurrencyModelId");

            migrationBuilder.RenameIndex(
                name: "IX_RealTimeCurrencyExchangeRates_CurrencyModel",
                table: "RealTimeCurrencyExchangeRates",
                newName: "IX_RealTimeCurrencyExchangeRates_CurrencyModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_RealTimeCurrencyExchangeRates_Currencies_CurrencyModelId",
                table: "RealTimeCurrencyExchangeRates",
                column: "CurrencyModelId",
                principalTable: "Currencies",
                principalColumn: "CurrencyModelId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RealTimeCurrencyExchangeRates_Currencies_CurrencyModelId",
                table: "RealTimeCurrencyExchangeRates");

            migrationBuilder.RenameColumn(
                name: "CurrencyModelId",
                table: "RealTimeCurrencyExchangeRates",
                newName: "CurrencyModel");

            migrationBuilder.RenameIndex(
                name: "IX_RealTimeCurrencyExchangeRates_CurrencyModelId",
                table: "RealTimeCurrencyExchangeRates",
                newName: "IX_RealTimeCurrencyExchangeRates_CurrencyModel");

            migrationBuilder.AddForeignKey(
                name: "FK_RealTimeCurrencyExchangeRates_Currencies_CurrencyModel",
                table: "RealTimeCurrencyExchangeRates",
                column: "CurrencyModel",
                principalTable: "Currencies",
                principalColumn: "CurrencyModelId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
