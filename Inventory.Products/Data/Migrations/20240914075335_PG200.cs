using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Products.Data.Migrations
{
    /// <inheritdoc />
    public partial class PG200 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Diff",
                schema: "Products",
                table: "QuantityMetric",
                type: "numeric(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsCancelled",
                schema: "Products",
                table: "QuantityMetric",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "TransactionId",
                schema: "Products",
                table: "QuantityMetric",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Diff",
                schema: "Products",
                table: "QuantityMetric");

            migrationBuilder.DropColumn(
                name: "IsCancelled",
                schema: "Products",
                table: "QuantityMetric");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                schema: "Products",
                table: "QuantityMetric");
        }
    }
}
