using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Prices.Data.Migrations
{
    /// <inheritdoc />
    public partial class pPricesv4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TargetCurrency",
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
                name: "TargetCurrency",
                schema: "Prices",
                table: "PricesParameter");
        }
    }
}
