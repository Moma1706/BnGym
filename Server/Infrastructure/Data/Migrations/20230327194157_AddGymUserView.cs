using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class AddGymUserView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW GymUserView AS
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
                    u.Address,
                    COALESCE(lastMonth.nalm, 0) AS [NumberOfArrivalsLastMonth],
                    COALESCE(currentMonth.nacm, 0) AS [NumberOfArrivalsCurrentMonth]
                    FROM GymUser gu
                    INNER JOIN Users u ON u.Id = gu.UserId
                    LEFT JOIN (
                        SELECT GymUserId, count(*) as nalm
                        FROM CheckInHistory
                        WHERE MONTH(TimeStamp) = MONTH(DATEADD(month, -1, GETDATE())) 
                        AND YEAR(TimeStamp) = YEAR(DATEADD(month, -1, GETDATE()))
                        GROUP BY GymUserId
                    ) AS lastMonth on lastMonth.GymUserId = gu.Id
                  LEFT JOIN (
                      SELECT GymUserId, count(*) as nacm
                      FROM CheckInHistory
                      WHERE MONTH(TimeStamp) = MONTH(GETDATE()) 
                      AND YEAR(TimeStamp) = YEAR(GETDATE())
                      GROUP BY GymUserId
                  ) AS currentMonth ON gu.Id = currentMonth.GymUserId;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW public.GymUserView;");
        }
    }
}
