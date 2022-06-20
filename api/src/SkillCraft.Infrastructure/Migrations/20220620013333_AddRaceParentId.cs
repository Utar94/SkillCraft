using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillCraft.Infrastructure.Migrations
{
    public partial class AddRaceParentId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Races",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Races_ParentId",
                table: "Races",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Races_Races_ParentId",
                table: "Races",
                column: "ParentId",
                principalTable: "Races",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Races_Races_ParentId",
                table: "Races");

            migrationBuilder.DropIndex(
                name: "IX_Races_ParentId",
                table: "Races");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Races");
        }
    }
}
