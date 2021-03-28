using Microsoft.EntityFrameworkCore.Migrations;

namespace MandobX.API.Migrations
{
    public partial class AddFirstNameAndLastName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Traders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Traders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Traders");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Traders");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Drivers");
        }
    }
}
