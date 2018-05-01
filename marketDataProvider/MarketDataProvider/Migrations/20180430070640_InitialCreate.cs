using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MarketDataProvider.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exchanges",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ProvidesFullHistoryData = table.Column<bool>(nullable: false),
                    Web = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exchanges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Markets",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CurrencyId = table.Column<string>(nullable: true),
                    MarketCurrencyId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Markets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Markets_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Markets_Currencies_MarketCurrencyId",
                        column: x => x.MarketCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExchangeCurrencies",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CurrencyExchangeId = table.Column<string>(nullable: true),
                    CurrencyId = table.Column<string>(nullable: true),
                    ExchangeId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeCurrencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExchangeCurrencies_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExchangeCurrencies_Exchanges_ExchangeId",
                        column: x => x.ExchangeId,
                        principalTable: "Exchanges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExchangeMarkets",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ExchangeId = table.Column<string>(nullable: true),
                    MarketExchangeId = table.Column<string>(nullable: true),
                    MarketId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeMarkets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExchangeMarkets_Exchanges_ExchangeId",
                        column: x => x.ExchangeId,
                        principalTable: "Exchanges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExchangeMarkets_Markets_MarketId",
                        column: x => x.MarketId,
                        principalTable: "Markets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeCurrencies_CurrencyId",
                table: "ExchangeCurrencies",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeCurrencies_ExchangeId",
                table: "ExchangeCurrencies",
                column: "ExchangeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeMarkets_ExchangeId",
                table: "ExchangeMarkets",
                column: "ExchangeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeMarkets_MarketId",
                table: "ExchangeMarkets",
                column: "MarketId");

            migrationBuilder.CreateIndex(
                name: "IX_Markets_CurrencyId",
                table: "Markets",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Markets_MarketCurrencyId",
                table: "Markets",
                column: "MarketCurrencyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExchangeCurrencies");

            migrationBuilder.DropTable(
                name: "ExchangeMarkets");

            migrationBuilder.DropTable(
                name: "Exchanges");

            migrationBuilder.DropTable(
                name: "Markets");

            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
