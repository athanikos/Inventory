using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Products.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProductsV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EffectiveDate",
                schema: "Products",
                table: "Metrics");

            migrationBuilder.DropColumn(
                name: "Value",
                schema: "Products",
                table: "Metrics");

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveDate",
                schema: "Products",
                table: "ProductMetrics",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Value",
                schema: "Products",
                table: "ProductMetrics",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EffectiveDate",
                schema: "Products",
                table: "ProductMetrics");

            migrationBuilder.DropColumn(
                name: "Value",
                schema: "Products",
                table: "ProductMetrics");

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveDate",
                schema: "Products",
                table: "Metrics",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Value",
                schema: "Products",
                table: "Metrics",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
