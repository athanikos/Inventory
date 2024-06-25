using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Expressions.Data.Migrations
{
    /// <inheritdoc />
    public partial class Exprv3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductExpression",
                schema: "Expressions",
                table: "ProductExpression");

            migrationBuilder.RenameTable(
                name: "ProductExpression",
                schema: "Expressions",
                newName: "BooleanExpression",
                newSchema: "Expressions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BooleanExpression",
                schema: "Expressions",
                table: "BooleanExpression",
                column: "Id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BooleanExpressions",
                schema: "Expressions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BooleanExpression",
                schema: "Expressions",
                table: "BooleanExpression");

            migrationBuilder.RenameTable(
                name: "BooleanExpression",
                schema: "Expressions",
                newName: "ProductExpression",
                newSchema: "Expressions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductExpression",
                schema: "Expressions",
                table: "ProductExpression",
                column: "Id");
        }
    }
}
