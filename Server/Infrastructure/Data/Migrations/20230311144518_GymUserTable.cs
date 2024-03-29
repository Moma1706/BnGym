﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class GymUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GymUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ExpiresOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    LastCheckIn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: DateTime.MinValue),
                    IsFrozen = table.Column<bool>(type: "bit", nullable: false),
                    IsInActive = table.Column<bool>(type: "bit", nullable: false),
                    FreezeDate = table.Column< DateTime > (type: "datetime2", nullable: true, defaultValue: DateTime.MinValue)
        },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GymUser", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GymUser");
        }
    }
}
