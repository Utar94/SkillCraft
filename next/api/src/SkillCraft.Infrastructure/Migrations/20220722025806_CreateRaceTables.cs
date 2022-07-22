using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SkillCraft.Infrastructure.Migrations
{
    public partial class CreateRaceTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Races",
                columns: table => new
                {
                    Sid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorldSid = table.Column<int>(type: "integer", nullable: false),
                    ParentSid = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Attributes = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ExtraAttributes = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    AttributesText = table.Column<string>(type: "text", nullable: true),
                    Names = table.Column<string>(type: "jsonb", nullable: true),
                    NamesText = table.Column<string>(type: "text", nullable: true),
                    Speeds = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SpeedsText = table.Column<string>(type: "text", nullable: true),
                    AgeThresholds = table.Column<int[]>(type: "integer[]", nullable: true),
                    Size = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    StatureRoll = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    WeightRolls = table.Column<string[]>(type: "character varying(10)[]", nullable: true),
                    AgeText = table.Column<string>(type: "text", nullable: true),
                    SizeText = table.Column<string>(type: "text", nullable: true),
                    WeightText = table.Column<string>(type: "text", nullable: true),
                    ExtraLanguages = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    LanguagesText = table.Column<string>(type: "text", nullable: true),
                    TraitsText = table.Column<string>(type: "text", nullable: true),
                    PeopleText = table.Column<string>(type: "text", nullable: true),
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Races", x => x.Sid);
                    table.ForeignKey(
                        name: "FK_Races_Races_ParentSid",
                        column: x => x.ParentSid,
                        principalTable: "Races",
                        principalColumn: "Sid");
                    table.ForeignKey(
                        name: "FK_Races_Worlds_WorldSid",
                        column: x => x.WorldSid,
                        principalTable: "Worlds",
                        principalColumn: "Sid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RaceLanguages",
                columns: table => new
                {
                    RaceSid = table.Column<int>(type: "integer", nullable: false),
                    LanguageSid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceLanguages", x => new { x.RaceSid, x.LanguageSid });
                    table.ForeignKey(
                        name: "FK_RaceLanguages_Languages_LanguageSid",
                        column: x => x.LanguageSid,
                        principalTable: "Languages",
                        principalColumn: "Sid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RaceLanguages_Races_RaceSid",
                        column: x => x.RaceSid,
                        principalTable: "Races",
                        principalColumn: "Sid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RacialTraits",
                columns: table => new
                {
                    Sid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    RaceSid = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RacialTraits", x => x.Sid);
                    table.ForeignKey(
                        name: "FK_RacialTraits_Races_RaceSid",
                        column: x => x.RaceSid,
                        principalTable: "Races",
                        principalColumn: "Sid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RaceLanguages_LanguageSid",
                table: "RaceLanguages",
                column: "LanguageSid");

            migrationBuilder.CreateIndex(
                name: "IX_Races_Id",
                table: "Races",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Races_Name",
                table: "Races",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Races_ParentSid",
                table: "Races",
                column: "ParentSid");

            migrationBuilder.CreateIndex(
                name: "IX_Races_Size",
                table: "Races",
                column: "Size");

            migrationBuilder.CreateIndex(
                name: "IX_Races_WorldSid",
                table: "Races",
                column: "WorldSid");

            migrationBuilder.CreateIndex(
                name: "IX_RacialTraits_Id",
                table: "RacialTraits",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RacialTraits_RaceSid",
                table: "RacialTraits",
                column: "RaceSid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RaceLanguages");

            migrationBuilder.DropTable(
                name: "RacialTraits");

            migrationBuilder.DropTable(
                name: "Races");
        }
    }
}
