using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SkillCraft.Infrastructure.Migrations
{
    public partial class CreatePowerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Powers",
                columns: table => new
                {
                    Sid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorldSid = table.Column<int>(type: "integer", nullable: false),
                    Tier = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descriptions = table.Column<string[]>(type: "text[]", nullable: true),
                    Incantation = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsRitual = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsSomatic = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsVerbal = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Duration = table.Column<int>(type: "integer", nullable: true),
                    IsConcentration = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Range = table.Column<int>(type: "integer", nullable: true),
                    Ingredients = table.Column<string>(type: "text", nullable: true),
                    IsFocus = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Powers", x => x.Sid);
                    table.ForeignKey(
                        name: "FK_Powers_Worlds_WorldSid",
                        column: x => x.WorldSid,
                        principalTable: "Worlds",
                        principalColumn: "Sid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Powers_Id",
                table: "Powers",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Powers_Name",
                table: "Powers",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Powers_Tier",
                table: "Powers",
                column: "Tier");

            migrationBuilder.CreateIndex(
                name: "IX_Powers_WorldSid",
                table: "Powers",
                column: "WorldSid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Powers");
        }
    }
}
