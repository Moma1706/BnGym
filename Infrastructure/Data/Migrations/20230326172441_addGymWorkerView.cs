using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class addGymWorkerView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                create or alter view GymWorkerView as
                select u.Id as UserId, u.FirstName, u.LastName, u.Email, u.IsBlocked, gw.Id, ur.RoleId
                from GymWorker gw
                inner join Users u on u.Id = gw.UserId
                inner join UserRoles ur on ur.UserId = u.Id;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW public.GymWorkerView;");
        }
    }
}
