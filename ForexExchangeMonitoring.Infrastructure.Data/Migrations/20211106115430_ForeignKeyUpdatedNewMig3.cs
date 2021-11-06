using Microsoft.EntityFrameworkCore.Migrations;

namespace ForexExchangeMonitoring.Infrastructure.Data.Migrations
{
    public partial class ForeignKeyUpdatedNewMig3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CurrencyModelId",
                table: "RealTimeCurrencyExchangeRates",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_RealTimeCurrencyExchangeRates_CurrencyModelId",
                table: "RealTimeCurrencyExchangeRates",
                column: "CurrencyModelId");

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

            migrationBuilder.DropIndex(
                name: "IX_RealTimeCurrencyExchangeRates_CurrencyModelId",
                table: "RealTimeCurrencyExchangeRates");

            migrationBuilder.AlterColumn<int>(
                name: "CurrencyModelId",
                table: "RealTimeCurrencyExchangeRates",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
