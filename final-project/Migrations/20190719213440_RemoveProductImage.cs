using Microsoft.EntityFrameworkCore.Migrations;

namespace finalproject.Migrations
{
    public partial class RemoveProductImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Img",
                table: "Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Img",
                table: "Products",
                nullable: true);
        }
    }
}
