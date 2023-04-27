﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class UpdateTypoErrorGymUserView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER VIEW GymUserView AS
                SELECT u.Id as UserId,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    u.IsBlocked,
                    gu.Id,
                    gu.ExpiresOn,
                    gu.IsFrozen,
                    gu.FreezeDate,
                    gu.IsInActive,
                    gu.LastCheckIn,
                    gu.Type,
                    gu.NumberOfArrivals,
                    u.Address
                from GymUser gu
                inner join Users u on u.Id = gu.UserId;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW public.GymUserView;");
        }
    }
}