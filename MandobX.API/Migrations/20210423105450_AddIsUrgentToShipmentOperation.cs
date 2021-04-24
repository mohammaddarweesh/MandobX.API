using Microsoft.EntityFrameworkCore.Migrations;

namespace MandobX.API.Migrations
{
    public partial class AddIsUrgentToShipmentOperation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUrgent",
                table: "ShipmentOperations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUrgent",
                table: "ShipmentOperations");
        }
    }
}
