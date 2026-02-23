using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectIndependence.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStockPropertyFromProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
