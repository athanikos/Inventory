using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Notifications.Data.Migrations
{
    /// <inheritdoc />
    public partial class iniial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Notifications");

            migrationBuilder.CreateTable(
                name: "Notification",
                schema: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BooleanExpressionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpressionValue = table.Column<bool>(type: "bit", nullable: false),
                    NotifyEveryMinutes = table.Column<int>(type: "int", nullable: false),
                    NotifyTimes = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notification",
                schema: "Notifications");
        }
    }
}
