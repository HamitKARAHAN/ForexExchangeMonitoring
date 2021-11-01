using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ForexExchange.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RealTimeCurrencyExchangeRates",
                columns: table => new
                {
                    RealTimeCurrencyExchangeRateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromCurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToCurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastRefreshedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealTimeCurrencyExchangeRates", x => x.RealTimeCurrencyExchangeRateId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RealTimeCurrencyExchangeRates");
        }
    }
}
