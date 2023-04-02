using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class addColumFreezeDateInGymUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfTrainingsLeft",
                table: "GymUser");

            migrationBuilder.AddColumn<DateTime>(
                name: "FreezeDate",
                table: "GymUser",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FreezeDate",
                table: "GymUser");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfTrainingsLeft",
                table: "GymUser",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
