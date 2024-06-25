using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Expressions.Data.Migrations
{
    /// <inheritdoc />
    public partial class Exprv4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BooleanExpressions",
                schema: "Expressions");

            migrationBuilder.DropColumn(
                name: "TargetMetricId",
                schema: "Expressions",
                table: "BooleanExpression");

            migrationBuilder.DropColumn(
                name: "TargetProductId",
                schema: "Expressions",
                table: "BooleanExpression");

            migrationBuilder.CreateTable(
                name: "ProductExpression",
                schema: "Expressions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Expression = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InventoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RunEveryMinutes = table.Column<int>(type: "int", nullable: false),
                    TargetProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetMetricId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductExpression", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductExpression",
                schema: "Expressions");

            migrationBuilder.AddColumn<Guid>(
                name: "TargetMetricId",
                schema: "Expressions",
                table: "BooleanExpression",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TargetProductId",
                schema: "Expressions",
                table: "BooleanExpression",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "BooleanExpressions",
                schema: "Expressions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Expression = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InventoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RunEveryMinutes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BooleanExpressions", x => x.Id);
                });
        }
    }
}
