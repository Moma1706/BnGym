using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class DailyHistoryView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE OR ALTER VIEW DailyHistoryView AS
            SELECT du.Id,
                du.FirstName,
                du.LastName,
                du.DateOfBirth,
                dh.CheckInDate
            FROM DailyUser du
            INNER JOIN DailyHistory dh ON dh.DailyUserId = du.Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW public.DailyHistoryView;");
        }
    }
}
