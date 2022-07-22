using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SkillCraft.Infrastructure.Migrations
{
    public partial class CreateTalentTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Talents",
                columns: table => new
                {
                    Sid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorldSid = table.Column<int>(type: "integer", nullable: false),
                    RequiredTalentSid = table.Column<int>(type: "integer", nullable: true),
                    Tier = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    MultipleAcquisition = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Skill = table.Column<int>(type: "integer", nullable: true),
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Talents", x => x.Sid);
                    table.ForeignKey(
                        name: "FK_Talents_Talents_RequiredTalentSid",
                        column: x => x.RequiredTalentSid,
                        principalTable: "Talents",
                        principalColumn: "Sid");
                    table.ForeignKey(
                        name: "FK_Talents_Worlds_WorldSid",
                        column: x => x.WorldSid,
                        principalTable: "Worlds",
                        principalColumn: "Sid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TalentOptions",
                columns: table => new
                {
                    Sid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    TalentSid = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TalentOptions", x => x.Sid);
                    table.ForeignKey(
                        name: "FK_TalentOptions_Talents_TalentSid",
                        column: x => x.TalentSid,
                        principalTable: "Talents",
                        principalColumn: "Sid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TalentOptions_Id",
                table: "TalentOptions",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TalentOptions_TalentSid",
                table: "TalentOptions",
                column: "TalentSid");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_Id",
                table: "Talents",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Talents_MultipleAcquisition",
                table: "Talents",
                column: "MultipleAcquisition");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_Name",
                table: "Talents",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_RequiredTalentSid",
                table: "Talents",
                column: "RequiredTalentSid");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_Skill",
                table: "Talents",
                column: "Skill");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_Tier",
                table: "Talents",
                column: "Tier");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_WorldSid",
                table: "Talents",
                column: "WorldSid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TalentOptions");

            migrationBuilder.DropTable(
                name: "Talents");
        }
    }
}
