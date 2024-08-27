using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Transactions.Data.Migrations
{
    /// <inheritdoc />
    public partial class PG6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TransactionType",
                schema: "Transactions",
                table: "Section",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionType",
                schema: "Transactions",
                table: "Section");
        }
    }
}
