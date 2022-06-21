using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillCraft.Infrastructure.Migrations
{
    public partial class AddSkillToTalentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Skill",
                table: "Talents",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Skill",
                table: "Talents");
        }
    }
}
