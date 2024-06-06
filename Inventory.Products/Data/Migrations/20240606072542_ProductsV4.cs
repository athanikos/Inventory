using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Products.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProductsV4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductMetrics",
                schema: "Products",
                table: "ProductMetrics");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductMetrics",
                schema: "Products",
                table: "ProductMetrics",
                columns: new[] { "MetricId", "ProductId", "EffectiveDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductMetrics",
                schema: "Products",
                table: "ProductMetrics");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductMetrics",
                schema: "Products",
                table: "ProductMetrics",
                columns: new[] { "MetricId", "ProductId" });
        }
    }
}
