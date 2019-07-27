using Microsoft.EntityFrameworkCore.Migrations;

namespace finalproject.Migrations
{
    public partial class updateOrder7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CCCvv",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CCExpiration",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CCName",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CCNumber",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zip",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CCCvv",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CCExpiration",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CCName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CCNumber",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Zip",
                table: "Orders");
        }
    }
}
