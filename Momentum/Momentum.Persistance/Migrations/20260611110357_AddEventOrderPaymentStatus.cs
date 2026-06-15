using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Momentum.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddEventOrderPaymentStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "EventOrders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidAt",
                table: "EventOrders",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "EventOrders");

            migrationBuilder.DropColumn(
                name: "PaidAt",
                table: "EventOrders");
        }
    }
}
