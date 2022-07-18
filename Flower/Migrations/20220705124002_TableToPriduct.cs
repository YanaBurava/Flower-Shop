using Microsoft.EntityFrameworkCore.Migrations;

namespace Flower.Migrations
{
    public partial class TableToPriduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TableId",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Product_TableId",
                table: "Product",
                column: "TableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Table_TableId",
                table: "Product",
                column: "TableId",
                principalTable: "Table",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Table_TableId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_TableId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "TableId",
                table: "Product");
        }
    }
}
