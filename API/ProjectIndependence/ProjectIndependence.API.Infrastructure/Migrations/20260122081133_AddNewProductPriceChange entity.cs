using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectIndependence.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewProductPriceChangeentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductPriceChanges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldRetailPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NewRetailPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OldCostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NewCostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReasonForPriceChange = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChangedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPriceChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPriceChanges_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductPriceChanges_ProductId",
                table: "ProductPriceChanges",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductPriceChanges");
        }
    }
}
