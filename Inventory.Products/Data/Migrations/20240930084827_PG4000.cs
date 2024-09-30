using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Products.Data.Migrations
{
    /// <inheritdoc />
    public partial class PG4000 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                schema: "Products",
                table: "UnitOfMeasurement",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                schema: "Products",
                table: "UnitOfMeasurement");
        }
    }
}
