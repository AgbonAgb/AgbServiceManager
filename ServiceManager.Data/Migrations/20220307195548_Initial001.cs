using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceManager.Data.Migrations
{
    public partial class Initial001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Srn = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceDesc = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Enddate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Daysnotification = table.Column<int>(type: "int", nullable: false),
                    Frequency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SetupBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactStaff = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Credentials = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Srn);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Services");
        }
    }
}
