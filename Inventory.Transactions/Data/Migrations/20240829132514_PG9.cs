using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Transactions.Data.Migrations
{
    /// <inheritdoc />
    public partial class PG9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Value_Entity_EntityId",
                schema: "Transactions",
                table: "Value");

            migrationBuilder.DropForeignKey(
                name: "FK_Value_Transaction_TransactionId",
                schema: "Transactions",
                table: "Value");

            migrationBuilder.DropTable(
                name: "Entity",
                schema: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Value_EntityId",
                schema: "Transactions",
                table: "Value");

            migrationBuilder.DropIndex(
                name: "IX_Value_TransactionId",
                schema: "Transactions",
                table: "Value");

            migrationBuilder.DropColumn(
                name: "EntityId",
                schema: "Transactions",
                table: "Value");

            migrationBuilder.DropColumn(
                name: "SectionLineNumber",
                schema: "Transactions",
                table: "Value");

            migrationBuilder.RenameColumn(
                name: "TransactionType",
                schema: "Transactions",
                table: "Section",
                newName: "SectionType");

            migrationBuilder.AddColumn<Guid>(
                name: "TransactionSectionGroupId",
                schema: "Transactions",
                table: "Value",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "TransactionSection",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionSectionType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionSection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionSection_Transaction_TransactionId",
                        column: x => x.TransactionId,
                        principalSchema: "Transactions",
                        principalTable: "Transaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionSectionGroup",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionSectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupValue = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionSectionGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionSectionGroup_TransactionSection_TransactionSecti~",
                        column: x => x.TransactionSectionId,
                        principalSchema: "Transactions",
                        principalTable: "TransactionSection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Value_TransactionSectionGroupId",
                schema: "Transactions",
                table: "Value",
                column: "TransactionSectionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionSection_TransactionId",
                schema: "Transactions",
                table: "TransactionSection",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionSectionGroup_TransactionSectionId",
                schema: "Transactions",
                table: "TransactionSectionGroup",
                column: "TransactionSectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Value_TransactionSectionGroup_TransactionSectionGroupId",
                schema: "Transactions",
                table: "Value",
                column: "TransactionSectionGroupId",
                principalSchema: "Transactions",
                principalTable: "TransactionSectionGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Value_TransactionSectionGroup_TransactionSectionGroupId",
                schema: "Transactions",
                table: "Value");

            migrationBuilder.DropTable(
                name: "TransactionSectionGroup",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "TransactionSection",
                schema: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Value_TransactionSectionGroupId",
                schema: "Transactions",
                table: "Value");

            migrationBuilder.DropColumn(
                name: "TransactionSectionGroupId",
                schema: "Transactions",
                table: "Value");

            migrationBuilder.RenameColumn(
                name: "SectionType",
                schema: "Transactions",
                table: "Section",
                newName: "TransactionType");

            migrationBuilder.AddColumn<Guid>(
                name: "EntityId",
                schema: "Transactions",
                table: "Value",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SectionLineNumber",
                schema: "Transactions",
                table: "Value",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Entity",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    EntityType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entity", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Value_EntityId",
                schema: "Transactions",
                table: "Value",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Value_TransactionId",
                schema: "Transactions",
                table: "Value",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Value_Entity_EntityId",
                schema: "Transactions",
                table: "Value",
                column: "EntityId",
                principalSchema: "Transactions",
                principalTable: "Entity",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Value_Transaction_TransactionId",
                schema: "Transactions",
                table: "Value",
                column: "TransactionId",
                principalSchema: "Transactions",
                principalTable: "Transaction",
                principalColumn: "Id");
        }
    }
}
