using Microsoft.EntityFrameworkCore.Migrations;

namespace ForexExchangeMonitoring.Infrastructure.Data.Migrations
{
    public partial class ForeignKeyUpdatedNewMig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "currencyModelId",
                table: "RealTimeCurrencyExchangeRates",
                newName: "CurrencyModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrencyModelId",
                table: "RealTimeCurrencyExchangeRates",
                newName: "currencyModelId");
        }
    }
}
