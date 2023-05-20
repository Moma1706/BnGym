using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class DailyUserView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW DailyUserView AS
                SELECT du.Id,
                    du.FirstName,
                    du.LastName,
                    du.DateOfBirth,
                    du.LastCheckIn,
                    COALESCE(lastMonth.nalm, 0) AS [NumberOfArrivalsLastMonth],
                    COALESCE(currentMonth.nacm, 0) AS [NumberOfArrivalsCurrentMonth]
                FROM DailyUser du
                LEFT JOIN (
                    SELECT DailyUserId, count(*) as nalm
                    FROM DailyHistory
                    WHERE MONTH(CheckInDate) = MONTH(DATEADD(month, -1, GETDATE())) 
                    AND YEAR(CheckInDate) = YEAR(DATEADD(month, -1, GETDATE()))
                    GROUP BY DailyUserId
                ) AS lastMonth on lastMonth.DailyUserId = du.Id
                LEFT JOIN (
                    SELECT DailyUserId, count(*) as nacm
                    FROM DailyHistory
                    WHERE MONTH(CheckInDate) = MONTH(GETDATE()) 
                    AND YEAR(CheckInDate) = YEAR(GETDATE())
                    GROUP BY DailyUserId
                ) AS currentMonth ON du.Id = currentMonth.DailyUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW public.DailyUserView;");
        }
    }
}
