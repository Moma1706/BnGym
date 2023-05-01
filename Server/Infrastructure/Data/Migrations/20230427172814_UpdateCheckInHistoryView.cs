using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class UpdateCheckInHistoryView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                create or alter view CheckInHistoryView as
                select ch.Id, ch.GymUserId, ch.TimeStamp, gu.LastCheckIn, u.Id as UserId, u.FirstName, u.LastName, u.Email
                from CheckInHistory ch
                inner join GymUser gu on ch.GymUserId = gu.Id
                inner join Users u on u.Id = gu.UserId;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW public.CheckInHistoryView;");
        }
    }
}
