using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillCraft.Infrastructure.Migrations
{
    public partial class RenamedSubraceTextToPeopleText : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubraceText",
                table: "Races",
                newName: "PeopleText");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PeopleText",
                table: "Races",
                newName: "SubraceText");
        }
    }
}
