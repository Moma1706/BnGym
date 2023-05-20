using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class AddCheckInHistoryView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW CheckInHistoryView AS
                    SELECT ch.Id, ch.GymUserId, ch.TimeStamp, gu.LastCheckIn, u.Id AS UserId, u.FirstName, u.LastName, u.Email
                    FROM CheckInHistory ch
                    INNER JOIN GymUser gu ON ch.GymUserId = gu.Id
                    INNER JOIN Users u ON u.Id = gu.UserId;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW public.CheckInHistoryView;");
        }
    }
}
