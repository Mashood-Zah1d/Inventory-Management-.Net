using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory_Management_.NET.Migrations
{
    /// <inheritdoc />
    public partial class DropAndRecreateProductIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop old ProductId column
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "purchaseHistories");

            // Add new ProductId column as GUID
            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "purchaseHistories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()"); // optional default
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverse: drop the GUID column and add back int
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "purchaseHistories");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "purchaseHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

    }
}
