using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Transactions.Data.Migrations
{
    /// <inheritdoc />
    public partial class PG5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Field_Template_TemplateId",
                schema: "Transactions",
                table: "Field");

            migrationBuilder.DropIndex(
                name: "IX_Field_TemplateId",
                schema: "Transactions",
                table: "Field");

            migrationBuilder.AddColumn<int>(
                name: "SectionLineNumber",
                schema: "Transactions",
                table: "Value",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "SectionId",
                schema: "Transactions",
                table: "Field",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Section",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Section", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Section_Template_TemplateId",
                        column: x => x.TemplateId,
                        principalSchema: "Transactions",
                        principalTable: "Template",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Field_SectionId",
                schema: "Transactions",
                table: "Field",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Section_TemplateId",
                schema: "Transactions",
                table: "Section",
                column: "TemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Field_Section_SectionId",
                schema: "Transactions",
                table: "Field",
                column: "SectionId",
                principalSchema: "Transactions",
                principalTable: "Section",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Field_Section_SectionId",
                schema: "Transactions",
                table: "Field");

            migrationBuilder.DropTable(
                name: "Section",
                schema: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Field_SectionId",
                schema: "Transactions",
                table: "Field");

            migrationBuilder.DropColumn(
                name: "SectionLineNumber",
                schema: "Transactions",
                table: "Value");

            migrationBuilder.DropColumn(
                name: "SectionId",
                schema: "Transactions",
                table: "Field");

            migrationBuilder.CreateIndex(
                name: "IX_Field_TemplateId",
                schema: "Transactions",
                table: "Field",
                column: "TemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Field_Template_TemplateId",
                schema: "Transactions",
                table: "Field",
                column: "TemplateId",
                principalSchema: "Transactions",
                principalTable: "Template",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
