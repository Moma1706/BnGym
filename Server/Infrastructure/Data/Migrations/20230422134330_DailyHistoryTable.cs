using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class DailyHistoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                  name: "DailyHistory",
                  columns: table => new
                  {
                      Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                      DailyUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                      CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                  },
                  constraints: table =>
                  {
                      table.PrimaryKey("PK_DailyHistory", x => x.Id);
                  });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyHistory");
        }
    }
}
