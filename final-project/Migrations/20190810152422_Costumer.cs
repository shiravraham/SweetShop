using Microsoft.EntityFrameworkCore.Migrations;

namespace finalproject.Migrations
{
    public partial class Costumer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Orders",
                newName: "CostumerEmail");

            migrationBuilder.AlterColumn<string>(
                name: "CostumerEmail",
                table: "Orders",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Costumer",
                columns: table => new
                {
                    Email = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Costumer", x => x.Email);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CostumerEmail",
                table: "Orders",
                column: "CostumerEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Costumer_CostumerEmail",
                table: "Orders",
                column: "CostumerEmail",
                principalTable: "Costumer",
                principalColumn: "Email",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Costumer_CostumerEmail",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Costumer");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CostumerEmail",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "CostumerEmail",
                table: "Orders",
                newName: "LastName");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Orders",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Orders",
                nullable: true);
        }
    }
}
