﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Transactions.Data.Migrations
{
    /// <inheritdoc />
    public partial class PG11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "Transactions",
                table: "TransactionSection",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Transactions",
                table: "TransactionSection");
        }
    }
}
