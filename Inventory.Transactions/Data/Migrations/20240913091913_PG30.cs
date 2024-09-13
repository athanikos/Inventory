using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Transactions.Data.Migrations
{
    /// <inheritdoc />
    public partial class PG30 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "TransactionId",
                schema: "Transactions",
                table: "Value",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Field_Template_TemplateId",
                schema: "Transactions",
                table: "Field");

            migrationBuilder.DropIndex(
                name: "IX_Field_TemplateId",
                schema: "Transactions",
                table: "Field");

            migrationBuilder.AlterColumn<Guid>(
                name: "TransactionId",
                schema: "Transactions",
                table: "Value",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }
    }
}
