using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Transactions.Data.Migrations
{
    /// <inheritdoc />
    public partial class PG10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TemplateId",
                schema: "Transactions",
                table: "Transaction",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_TemplateId",
                schema: "Transactions",
                table: "Transaction",
                column: "TemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Template_TemplateId",
                schema: "Transactions",
                table: "Transaction",
                column: "TemplateId",
                principalSchema: "Transactions",
                principalTable: "Template",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Template_TemplateId",
                schema: "Transactions",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_TemplateId",
                schema: "Transactions",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "TemplateId",
                schema: "Transactions",
                table: "Transaction");
        }
    }
}
