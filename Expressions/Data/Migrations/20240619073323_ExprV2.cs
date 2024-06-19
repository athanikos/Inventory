using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Expressions.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExprV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductExpressions",
                schema: "Expressions");

            migrationBuilder.RenameColumn(
                name: "TargetInventoryId",
                schema: "Expressions",
                table: "ProductExpression",
                newName: "TargetProductId");

            migrationBuilder.AddColumn<Guid>(
                name: "InventoryId",
                schema: "Expressions",
                table: "ProductExpression",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "InventoryExpression",
                schema: "Expressions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Expression = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RunEveryMinutes = table.Column<int>(type: "int", nullable: false),
                    TargetInventoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetMetricId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryExpression", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryExpression",
                schema: "Expressions");

            migrationBuilder.DropColumn(
                name: "InventoryId",
                schema: "Expressions",
                table: "ProductExpression");

            migrationBuilder.RenameColumn(
                name: "TargetProductId",
                schema: "Expressions",
                table: "ProductExpression",
                newName: "TargetInventoryId");

            migrationBuilder.CreateTable(
                name: "ProductExpressions",
                schema: "Expressions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Expression = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InventoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RunEveryMinutes = table.Column<int>(type: "int", nullable: false),
                    TargetMetricId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductExpressions", x => x.Id);
                });
        }
    }
}
