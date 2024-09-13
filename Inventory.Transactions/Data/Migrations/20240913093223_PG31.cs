using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Transactions.Data.Migrations
{
    /// <inheritdoc />
    public partial class PG31 : Migration
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

            migrationBuilder.DropColumn(
                name: "TemplateId",
                schema: "Transactions",
                table: "Field");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TemplateId",
                schema: "Transactions",
                table: "Field",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
