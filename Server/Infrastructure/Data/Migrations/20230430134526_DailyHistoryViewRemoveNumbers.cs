using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class DailyHistoryViewRemoveNumbers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE OR ALTER VIEW DailyHistoryView AS
            SELECT dt.Id,
                dt.FirstName,
                dt.LastName,
                dt.DateOfBirth,
                dh.CheckInDate
            FROM DailyTraining dt
            INNER JOIN DailyHistory dh ON dh.DailyUserId = dt.Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW public.DailyHistoryView;");
        }
    }
}
