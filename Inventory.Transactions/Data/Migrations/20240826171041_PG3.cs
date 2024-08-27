using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Transactions.Data.Migrations
{
    /// <inheritdoc />
    public partial class PG3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                schema: "Transactions",
                table: "Transactions");

            migrationBuilder.RenameTable(
                name: "Transactions",
                schema: "Transactions",
                newName: "Transaction",
                newSchema: "Transactions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transaction",
                schema: "Transactions",
                table: "Transaction",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Entity",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Template",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Template", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Field",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Expression = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Field", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Field_Template_TemplateId",
                        column: x => x.TemplateId,
                        principalSchema: "Transactions",
                        principalTable: "Template",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Value",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    FieldId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uuid", nullable: true),
                    EntityId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Value", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Value_Entity_EntityId",
                        column: x => x.EntityId,
                        principalSchema: "Transactions",
                        principalTable: "Entity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Value_Field_FieldId",
                        column: x => x.FieldId,
                        principalSchema: "Transactions",
                        principalTable: "Field",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Value_Transaction_TransactionId",
                        column: x => x.TransactionId,
                        principalSchema: "Transactions",
                        principalTable: "Transaction",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Field_TemplateId",
                schema: "Transactions",
                table: "Field",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Value_EntityId",
                schema: "Transactions",
                table: "Value",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Value_FieldId",
                schema: "Transactions",
                table: "Value",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_Value_TransactionId",
                schema: "Transactions",
                table: "Value",
                column: "TransactionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Value",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "Entity",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "Field",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "Template",
                schema: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transaction",
                schema: "Transactions",
                table: "Transaction");

            migrationBuilder.RenameTable(
                name: "Transaction",
                schema: "Transactions",
                newName: "Transactions",
                newSchema: "Transactions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                schema: "Transactions",
                table: "Transactions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TransactionItems",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uuid", nullable: false)
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
                name: "TransactionItemTemplateField",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Expression = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    TransactionItemTemplateId = table.Column<Guid>(type: "uuid", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false)
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
                name: "Values",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionItemTemplateFieldId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionItemId = table.Column<Guid>(type: "uuid", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: false)
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
    }
}
