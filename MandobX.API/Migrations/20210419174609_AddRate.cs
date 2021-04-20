﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace MandobX.API.Migrations
{
    public partial class AddRate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Rate",
                table: "Drivers",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Drivers");
        }
    }
}
