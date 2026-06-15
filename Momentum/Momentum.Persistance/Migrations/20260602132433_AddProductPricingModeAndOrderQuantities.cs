using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Momentum.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddProductPricingModeAndOrderQuantities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHourly",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPerPerson",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfHours",
                table: "EventOrderProductEntity",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfPeople",
                table: "EventOrderProductEntity",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHourly",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsPerPerson",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NumberOfHours",
                table: "EventOrderProductEntity");

            migrationBuilder.DropColumn(
                name: "NumberOfPeople",
                table: "EventOrderProductEntity");
        }
    }
}
