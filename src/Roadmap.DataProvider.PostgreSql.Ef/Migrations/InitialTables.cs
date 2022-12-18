using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Roadmap.Models.Db;

namespace Roadmap.DataProvider.PostgreSql.Ef.Migrations;

[DbContext(typeof(RoadmapDbContext))]
[Migration("20220519114511_InitialTables")]
class InitialTables : Migration
{
    protected override void Up(MigrationBuilder builder)
    {
        builder.CreateTable(
            name: DbUser.TableName,
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                FirstName = table.Column<string>(nullable: false),
                LastName = table.Column<string>(nullable: false),
                MiddleName = table.Column<string>(nullable: true),
                Status = table.Column<int>(nullable: false),
                IsAdmin = table.Column<bool>(nullable: false),
                IsActive = table.Column<bool>(nullable: false),
                CreatedBy = table.Column<Guid>(nullable: false),
                CreatedAtUtc = table.Column<DateTime>(nullable: false),
                ModifiedBy = table.Column<Guid>(nullable: true),
                ModifiedAtUtc = table.Column<DateTime>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder builder)
    {
        builder.DropTable(DbUser.TableName);
    }
}
