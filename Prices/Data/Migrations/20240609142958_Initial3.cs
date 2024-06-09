using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Prices.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Prices",
                table: "PricesParameter",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "MetricId",
                schema: "Prices",
                table: "PricesParameter",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "RunEveryMinutes",
                schema: "Prices",
                table: "PricesParameter",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TargetKey",
                schema: "Prices",
                table: "PricesParameter",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TargetPathForProductCode",
                schema: "Prices",
                table: "PricesParameter",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TargetProductCode",
                schema: "Prices",
                table: "PricesParameter",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TargetURL",
                schema: "Prices",
                table: "PricesParameter",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Prices",
                table: "PricesParameter");

            migrationBuilder.DropColumn(
                name: "MetricId",
                schema: "Prices",
                table: "PricesParameter");

            migrationBuilder.DropColumn(
                name: "RunEveryMinutes",
                schema: "Prices",
                table: "PricesParameter");

            migrationBuilder.DropColumn(
                name: "TargetKey",
                schema: "Prices",
                table: "PricesParameter");

            migrationBuilder.DropColumn(
                name: "TargetPathForProductCode",
                schema: "Prices",
                table: "PricesParameter");

            migrationBuilder.DropColumn(
                name: "TargetProductCode",
                schema: "Prices",
                table: "PricesParameter");

            migrationBuilder.DropColumn(
                name: "TargetURL",
                schema: "Prices",
                table: "PricesParameter");
        }
    }
}
