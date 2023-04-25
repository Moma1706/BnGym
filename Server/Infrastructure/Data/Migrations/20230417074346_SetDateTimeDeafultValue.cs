using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class SetDateTimeDeafultValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE dbo.GymUser ADD CONSTRAINT DF_GymUser_FreezeDate DEFAULT '0001-01-01' FOR FreezeDate");
            migrationBuilder.Sql("ALTER TABLE dbo.GymUser ADD CONSTRAINT DF_GymUser_LastCheckIn DEFAULT '0001-01-01' FOR LastCheckIn");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE dbo.GymUser DROP CONSTRAINT DF_GymUser_FreezeDate");
            migrationBuilder.Sql("ALTER TABLE dbo.GymUser DROP CONSTRAINT DF_GymUser_LastCheckIn");
        }
    }
}
