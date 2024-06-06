using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Products.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProductsV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategory_Category_CategoryId",
                schema: "Products",
                table: "ProductCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategory_Product_ProductId",
                schema: "Products",
                table: "ProductCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductMetric_Metric_MetricId",
                schema: "Products",
                table: "ProductMetric");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductMetric_Product_ProductId",
                schema: "Products",
                table: "ProductMetric");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductMetric",
                schema: "Products",
                table: "ProductMetric");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCategory",
                schema: "Products",
                table: "ProductCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Metric",
                schema: "Products",
                table: "Metric");

            migrationBuilder.RenameTable(
                name: "ProductMetric",
                schema: "Products",
                newName: "ProductMetrics",
                newSchema: "Products");

            migrationBuilder.RenameTable(
                name: "ProductCategory",
                schema: "Products",
                newName: "ProductCategories",
                newSchema: "Products");

            migrationBuilder.RenameTable(
                name: "Metric",
                schema: "Products",
                newName: "Metrics",
                newSchema: "Products");

            migrationBuilder.RenameIndex(
                name: "IX_ProductMetric_ProductId",
                schema: "Products",
                table: "ProductMetrics",
                newName: "IX_ProductMetrics_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCategory_ProductId",
                schema: "Products",
                table: "ProductCategories",
                newName: "IX_ProductCategories_ProductId");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "Products",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductMetrics",
                schema: "Products",
                table: "ProductMetrics",
                columns: new[] { "MetricId", "ProductId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCategories",
                schema: "Products",
                table: "ProductCategories",
                columns: new[] { "CategoryId", "ProductId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Metrics",
                schema: "Products",
                table: "Metrics",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Source",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Source", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionItems",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriceAfterVat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionFees = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeliveryFees = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FinalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionItems_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalSchema: "Products",
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Metrics_SourceId",
                schema: "Products",
                table: "Metrics",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionItems_TransactionId",
                schema: "Products",
                table: "TransactionItems",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Metrics_Source_SourceId",
                schema: "Products",
                table: "Metrics",
                column: "SourceId",
                principalSchema: "Products",
                principalTable: "Source",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Category_CategoryId",
                schema: "Products",
                table: "ProductCategories",
                column: "CategoryId",
                principalSchema: "Products",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Product_ProductId",
                schema: "Products",
                table: "ProductCategories",
                column: "ProductId",
                principalSchema: "Products",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMetrics_Metrics_MetricId",
                schema: "Products",
                table: "ProductMetrics",
                column: "MetricId",
                principalSchema: "Products",
                principalTable: "Metrics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMetrics_Product_ProductId",
                schema: "Products",
                table: "ProductMetrics",
                column: "ProductId",
                principalSchema: "Products",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Metrics_Source_SourceId",
                schema: "Products",
                table: "Metrics");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Category_CategoryId",
                schema: "Products",
                table: "ProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Product_ProductId",
                schema: "Products",
                table: "ProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductMetrics_Metrics_MetricId",
                schema: "Products",
                table: "ProductMetrics");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductMetrics_Product_ProductId",
                schema: "Products",
                table: "ProductMetrics");

            migrationBuilder.DropTable(
                name: "Source",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "TransactionItems",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "Transactions",
                schema: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductMetrics",
                schema: "Products",
                table: "ProductMetrics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCategories",
                schema: "Products",
                table: "ProductCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Metrics",
                schema: "Products",
                table: "Metrics");

            migrationBuilder.DropIndex(
                name: "IX_Metrics_SourceId",
                schema: "Products",
                table: "Metrics");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Products",
                table: "Product");

            migrationBuilder.RenameTable(
                name: "ProductMetrics",
                schema: "Products",
                newName: "ProductMetric",
                newSchema: "Products");

            migrationBuilder.RenameTable(
                name: "ProductCategories",
                schema: "Products",
                newName: "ProductCategory",
                newSchema: "Products");

            migrationBuilder.RenameTable(
                name: "Metrics",
                schema: "Products",
                newName: "Metric",
                newSchema: "Products");

            migrationBuilder.RenameIndex(
                name: "IX_ProductMetrics_ProductId",
                schema: "Products",
                table: "ProductMetric",
                newName: "IX_ProductMetric_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCategories_ProductId",
                schema: "Products",
                table: "ProductCategory",
                newName: "IX_ProductCategory_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductMetric",
                schema: "Products",
                table: "ProductMetric",
                columns: new[] { "MetricId", "ProductId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCategory",
                schema: "Products",
                table: "ProductCategory",
                columns: new[] { "CategoryId", "ProductId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Metric",
                schema: "Products",
                table: "Metric",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategory_Category_CategoryId",
                schema: "Products",
                table: "ProductCategory",
                column: "CategoryId",
                principalSchema: "Products",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategory_Product_ProductId",
                schema: "Products",
                table: "ProductCategory",
                column: "ProductId",
                principalSchema: "Products",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMetric_Metric_MetricId",
                schema: "Products",
                table: "ProductMetric",
                column: "MetricId",
                principalSchema: "Products",
                principalTable: "Metric",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMetric_Product_ProductId",
                schema: "Products",
                table: "ProductMetric",
                column: "ProductId",
                principalSchema: "Products",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
