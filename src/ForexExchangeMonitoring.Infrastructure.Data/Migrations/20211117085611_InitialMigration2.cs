using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ForexExchangeMonitoring.Infrastructure.Data.Migrations
{
    public partial class InitialMigration2 : Migration
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
                name: "CurrencyExchangeRatesLive",
                columns: table => new
                {
                    currency_exchange_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    from_currency_id = table.Column<int>(type: "int", nullable: true),
                    to_currency_id = table.Column<int>(type: "int", nullable: true),
                    exchange_rate = table.Column<double>(type: "float", nullable: false),
                    last_refreshed_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyExchangeRatesLive", x => x.currency_exchange_id);
                    table.ForeignKey(
                        name: "FK_CurrencyExchangeRatesLive_Currencies_from_currency_id",
                        column: x => x.from_currency_id,
                        principalTable: "Currencies",
                        principalColumn: "currency_Id");
                    table.ForeignKey(
                        name: "FK_CurrencyExchangeRatesLive_Currencies_to_currency_id",
                        column: x => x.to_currency_id,
                        principalTable: "Currencies",
                        principalColumn: "currency_Id");
                });

            migrationBuilder.CreateTable(
                name: "CurrencyExchangeRatesHistory",
                columns: table => new
                {
                    currency_exchange_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    from_currency_id = table.Column<int>(type: "int", nullable: true),
                    to_currency_id = table.Column<int>(type: "int", nullable: true),
                    exchange_rate = table.Column<double>(type: "float", nullable: false),
                    last_refreshed_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ForexCurrencyModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyExchangeRatesHistory", x => x.currency_exchange_id);
                    table.ForeignKey(
                        name: "FK_CurrencyExchangeRatesHistory_Currencies_from_currency_id",
                        column: x => x.from_currency_id,
                        principalTable: "Currencies",
                        principalColumn: "currency_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CurrencyExchangeRatesHistory_Currencies_to_currency_id",
                        column: x => x.to_currency_id,
                        principalTable: "Currencies",
                        principalColumn: "currency_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CurrencyExchangeRatesHistory_CurrencyExchangeRatesLive_ForexCurrencyModelId",
                        column: x => x.ForexCurrencyModelId,
                        principalTable: "CurrencyExchangeRatesLive",
                        principalColumn: "currency_exchange_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyExchangeRatesHistory_ForexCurrencyModelId",
                table: "CurrencyExchangeRatesHistory",
                column: "ForexCurrencyModelId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyExchangeRatesHistory_from_currency_id",
                table: "CurrencyExchangeRatesHistory",
                column: "from_currency_id");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyExchangeRatesHistory_to_currency_id",
                table: "CurrencyExchangeRatesHistory",
                column: "to_currency_id");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyExchangeRatesLive_from_currency_id",
                table: "CurrencyExchangeRatesLive",
                column: "from_currency_id");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyExchangeRatesLive_to_currency_id",
                table: "CurrencyExchangeRatesLive",
                column: "to_currency_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyExchangeRatesHistory");

            migrationBuilder.DropTable(
                name: "CurrencyExchangeRatesLive");

            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
