using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Expressions.Data.Migrations
{
    /// <inheritdoc />
    public partial class PG : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Expressions");

            migrationBuilder.CreateTable(
                name: "BooleanExpression",
                schema: "Expressions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Expression = table.Column<string>(type: "text", nullable: false),
                    InventoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    RunEveryMinutes = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BooleanExpression", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryExpression",
                schema: "Expressions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Expression = table.Column<string>(type: "text", nullable: false),
                    RunEveryMinutes = table.Column<int>(type: "integer", nullable: false),
                    TargetInventoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetMetricId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryExpression", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductExpression",
                schema: "Expressions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Expression = table.Column<string>(type: "text", nullable: false),
                    InventoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    RunEveryMinutes = table.Column<int>(type: "integer", nullable: false),
                    TargetProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetMetricId = table.Column<Guid>(type: "uuid", nullable: false)
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
                name: "BooleanExpression",
                schema: "Expressions");

            migrationBuilder.DropTable(
                name: "InventoryExpression",
                schema: "Expressions");

            migrationBuilder.DropTable(
                name: "ProductExpression",
                schema: "Expressions");
        }
    }
}
