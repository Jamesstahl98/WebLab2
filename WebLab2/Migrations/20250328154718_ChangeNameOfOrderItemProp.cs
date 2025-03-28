using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebLab2.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNameOfOrderItemProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "OrderItem",
                newName: "ProductUnitPrice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductUnitPrice",
                table: "OrderItem",
                newName: "Price");
        }
    }
}
