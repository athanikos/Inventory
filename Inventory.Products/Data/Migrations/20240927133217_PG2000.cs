using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Products.Data.Migrations
{
    /// <inheritdoc />
    public partial class PG2000 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                schema: "Products",
                table: "ProductMetrics");

            migrationBuilder.DropColumn(
                name: "Currency",
                schema: "Products",
                table: "InventoryMetrics");

            migrationBuilder.AddColumn<Guid>(
                name: "UnitOfMeasurementId",
                schema: "Products",
                table: "ProductMetrics",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UnitOfMeasurementId",
                schema: "Products",
                table: "InventoryMetrics",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitOfMeasurementId",
                schema: "Products",
                table: "ProductMetrics");

            migrationBuilder.DropColumn(
                name: "UnitOfMeasurementId",
                schema: "Products",
                table: "InventoryMetrics");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                schema: "Products",
                table: "ProductMetrics",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                schema: "Products",
                table: "InventoryMetrics",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
