using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class addGymWorkerView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW GymWorkerView AS
                    SELECT u.Id AS UserId, u.FirstName, u.LastName, u.Email, u.IsBlocked, gw.Id, ur.RoleId
                    FROM GymWorker gw
                    INNER JOIN Users u ON u.Id = gw.UserId
                    INNER JOIN UserRoles ur ON ur.UserId = u.Id;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW public.GymWorkerView;");
        }
    }
}
