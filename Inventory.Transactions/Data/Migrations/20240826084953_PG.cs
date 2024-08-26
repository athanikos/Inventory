using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Transactions.Data.Migrations
{
    /// <inheritdoc />
    public partial class PG : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Transactions");

            migrationBuilder.CreateTable(
                name: "TransactionItemTemplate",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionItemTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionItemTemplateField",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Expression = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    TransactionItemTemplateId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionItemTemplateField", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionItemTemplateField_TransactionItemTemplate_Transa~",
                        column: x => x.TransactionItemTemplateId,
                        principalSchema: "Transactions",
                        principalTable: "TransactionItemTemplate",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TransactionItems",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionItems_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalSchema: "Transactions",
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Values",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    TransactionItemTemplateFieldId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionItemId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Values", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Values_TransactionItemTemplateField_TransactionItemTemplate~",
                        column: x => x.TransactionItemTemplateFieldId,
                        principalSchema: "Transactions",
                        principalTable: "TransactionItemTemplateField",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Values_TransactionItems_TransactionItemId",
                        column: x => x.TransactionItemId,
                        principalSchema: "Transactions",
                        principalTable: "TransactionItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionItems_TransactionId",
                schema: "Transactions",
                table: "TransactionItems",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionItemTemplateField_TransactionItemTemplateId",
                schema: "Transactions",
                table: "TransactionItemTemplateField",
                column: "TransactionItemTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Values_TransactionItemId",
                schema: "Transactions",
                table: "Values",
                column: "TransactionItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Values_TransactionItemTemplateFieldId",
                schema: "Transactions",
                table: "Values",
                column: "TransactionItemTemplateFieldId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Values",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "TransactionItemTemplateField",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "TransactionItems",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "TransactionItemTemplate",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "Transactions",
                schema: "Transactions");
        }
    }
}
