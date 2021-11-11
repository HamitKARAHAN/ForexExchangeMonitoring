using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ForexExchangeMonitoring.Infrastructure.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    currency_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    currency_name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.currency_Id);
                });

            migrationBuilder.CreateTable(
                name: "RealTimeCurrencyExchangeRates",
                columns: table => new
                {
                    forex_currency_model_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    from_currency_id = table.Column<int>(type: "int", nullable: false),
                    to_currency_id = table.Column<int>(type: "int", nullable: false),
                    ExchangeRate = table.Column<double>(type: "float", nullable: false),
                    LastRefreshedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealTimeCurrencyExchangeRates", x => x.forex_currency_model_id);
                    table.ForeignKey(
                        name: "FK_RealTimeCurrencyExchangeRates_Currencies_from_currency_id",
                        column: x => x.from_currency_id,
                        principalTable: "Currencies",
                        principalColumn: "currency_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RealTimeCurrencyExchangeRates_Currencies_to_currency_id",
                        column: x => x.to_currency_id,
                        principalTable: "Currencies",
                        principalColumn: "currency_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RealTimeCurrencyExchangeRates_from_currency_id",
                table: "RealTimeCurrencyExchangeRates",
                column: "from_currency_id");

            migrationBuilder.CreateIndex(
                name: "IX_RealTimeCurrencyExchangeRates_to_currency_id",
                table: "RealTimeCurrencyExchangeRates",
                column: "to_currency_id");
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
