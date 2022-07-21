using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SkillCraft.Infrastructure.Migrations
{
    public partial class CreateAspectTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aspects",
                columns: table => new
                {
                    Sid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorldSid = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    MandatoryAttribute1 = table.Column<int>(type: "integer", nullable: false),
                    MandatoryAttribute2 = table.Column<int>(type: "integer", nullable: false),
                    OptionalAttribute1 = table.Column<int>(type: "integer", nullable: false),
                    OptionalAttribute2 = table.Column<int>(type: "integer", nullable: false),
                    Skill1 = table.Column<int>(type: "integer", nullable: false),
                    Skill2 = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aspects", x => x.Sid);
                    table.ForeignKey(
                        name: "FK_Aspects_Worlds_WorldSid",
                        column: x => x.WorldSid,
                        principalTable: "Worlds",
                        principalColumn: "Sid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aspects_Id",
                table: "Aspects",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Aspects_Name",
                table: "Aspects",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Aspects_WorldSid",
                table: "Aspects",
                column: "WorldSid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aspects");
        }
    }
}
