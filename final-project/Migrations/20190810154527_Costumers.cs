using Microsoft.EntityFrameworkCore.Migrations;

namespace finalproject.Migrations
{
    public partial class Costumers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Costumer_CostumerEmail",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Costumer",
                table: "Costumer");

            migrationBuilder.RenameTable(
                name: "Costumer",
                newName: "Costumers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Costumers",
                table: "Costumers",
                column: "Email");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Costumers_CostumerEmail",
                table: "Orders",
                column: "CostumerEmail",
                principalTable: "Costumers",
                principalColumn: "Email",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Costumers_CostumerEmail",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Costumers",
                table: "Costumers");

            migrationBuilder.RenameTable(
                name: "Costumers",
                newName: "Costumer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Costumer",
                table: "Costumer",
                column: "Email");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Costumer_CostumerEmail",
                table: "Orders",
                column: "CostumerEmail",
                principalTable: "Costumer",
                principalColumn: "Email",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
