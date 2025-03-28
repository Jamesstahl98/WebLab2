using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebLab2.Migrations
{
    /// <inheritdoc />
    public partial class AddConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_User_Email",
                table: "Users",
                sql: "[Email] LIKE '%@%.%'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Product_Price",
                table: "Products",
                sql: "[Price] >= 0.01 AND [Price] <= 10000");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_User_Email",
                table: "Users");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Product_Price",
                table: "Products");
        }
    }
}
