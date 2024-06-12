using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Products.Data.Migrations
{
    /// <inheritdoc />
    public partial class Productsv5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Currency",
                schema: "Products",
                table: "ProductMetrics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                schema: "Products",
                table: "ProductMetrics");
        }
    }
}
