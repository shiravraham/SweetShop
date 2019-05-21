using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace final_project.Data
{
    public partial class initial : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Airplane",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                Capacity = table.Column<int>(nullable: false),
                Model = table.Column<string>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Airplane", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Airport",
            columns: table => new
            {
                ID = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                Country = table.Column<string>(nullable: true),
                City = table.Column<string>(nullable: true),
                Longitude = table.Column<double>(nullable: false),
                Latitude = table.Column<double>(nullable: false),
                Acronyms = table.Column<string>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Airport", x => x.ID);
            });

        migrationBuilder.CreateTable(
            name: "User",
            columns: table => new
            {
                ID = table.Column<int>(nullable: false),
                UserName = table.Column<string>(nullable: false),
                Password = table.Column<string>(nullable: false),
                FirstName = table.Column<string>(nullable: false),
                LastName = table.Column<string>(nullable: false),
                Age = table.Column<int>(nullable: false),
                Email = table.Column<string>(nullable: false),
                IsManager = table.Column<bool>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_User", x => x.ID);
            });

        migrationBuilder.CreateTable(
            name: "Flight",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                DestAirportID = table.Column<int>(nullable: true),
                SourceAirportID = table.Column<int>(nullable: true),
                AirplaneId = table.Column<int>(nullable: false),
                Date = table.Column<DateTime>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Flight", x => x.Id);
                table.ForeignKey(
                    name: "FK_Flight_Airplane_AirplaneId",
                    column: x => x.AirplaneId,
                    principalTable: "Airplane",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Flight_Airport_DestAirportID",
                    column: x => x.DestAirportID,
                    principalTable: "Airport",
                    principalColumn: "ID",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Flight_Airport_SourceAirportID",
                    column: x => x.SourceAirportID,
                    principalTable: "Airport",
                    principalColumn: "ID",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Ticket",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                FlightID = table.Column<int>(nullable: false),
                Price = table.Column<int>(nullable: false),
                LuggageWeight = table.Column<int>(nullable: false),
                BuyerID = table.Column<int>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Ticket", x => x.Id);
                table.ForeignKey(
                    name: "FK_Ticket_User_BuyerID",
                    column: x => x.BuyerID,
                    principalTable: "User",
                    principalColumn: "ID",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Ticket_Flight_FlightID",
                    column: x => x.FlightID,
                    principalTable: "Flight",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Flight_AirplaneId",
            table: "Flight",
            column: "AirplaneId");

        migrationBuilder.CreateIndex(
            name: "IX_Flight_DestAirportID",
            table: "Flight",
            column: "DestAirportID");

        migrationBuilder.CreateIndex(
            name: "IX_Flight_SourceAirportID",
            table: "Flight",
            column: "SourceAirportID");

        migrationBuilder.CreateIndex(
            name: "IX_Ticket_BuyerID",
            table: "Ticket",
            column: "BuyerID");

        migrationBuilder.CreateIndex(
            name: "IX_Ticket_FlightID",
            table: "Ticket",
            column: "FlightID");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Ticket");

        migrationBuilder.DropTable(
            name: "User");

        migrationBuilder.DropTable(
            name: "Flight");

        migrationBuilder.DropTable(
            name: "Airplane");

        migrationBuilder.DropTable(
            name: "Airport");
    }
}
}
