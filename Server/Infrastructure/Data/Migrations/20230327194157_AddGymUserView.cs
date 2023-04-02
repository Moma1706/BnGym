using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class AddGymUserView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                create or alter view GymUserView as
                select u.Id as UserId,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    u.IsBlocked,
                    gu.Id,
                    gu.IsStudent,
                    gu.ExpiresOn,
                    gu.IsFrozen,
                    gu.FreezeDate,
                    gu.IsInactive,
                    gu.LastCheckIn,
                    gu.Type,
                    gu.NumberOfArrivals
                from GymUser gu
                inner join Users u on u.Id = gu.UserId;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW public.GymUserView;");
        }
    }
}
