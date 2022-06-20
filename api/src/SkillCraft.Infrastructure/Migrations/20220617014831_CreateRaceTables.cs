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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorldId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Attributes = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Names = table.Column<string>(type: "jsonb", nullable: true),
                    Speeds = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    AgeThresholds = table.Column<int[]>(type: "integer[]", nullable: true),
                    Size = table.Column<int>(type: "integer", nullable: false),
                    StatureRoll = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    WeightRolls = table.Column<string[]>(type: "character varying(10)[]", nullable: true),
                    Traits = table.Column<string>(type: "jsonb", nullable: true),
                    ExtraAttributes = table.Column<int>(type: "integer", nullable: false),
                    ExtraLanguages = table.Column<int>(type: "integer", nullable: false),
                    AgeText = table.Column<string>(type: "text", nullable: true),
                    AttributesText = table.Column<string>(type: "text", nullable: true),
                    LanguagesText = table.Column<string>(type: "text", nullable: true),
                    NamesText = table.Column<string>(type: "text", nullable: true),
                    SizeText = table.Column<string>(type: "text", nullable: true),
                    SpeedText = table.Column<string>(type: "text", nullable: true),
                    SubraceText = table.Column<string>(type: "text", nullable: true),
                    TraitsText = table.Column<string>(type: "text", nullable: true),
                    WeightText = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedById = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Version = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Races", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Races_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RaceLanguages",
                columns: table => new
                {
                    RaceId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceLanguages", x => new { x.RaceId, x.LanguageId });
                    table.ForeignKey(
                        name: "FK_RaceLanguages_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RaceLanguages_Races_RaceId",
                        column: x => x.RaceId,
                        principalTable: "Races",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RaceLanguages_LanguageId",
                table: "RaceLanguages",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Races_CreatedById",
                table: "Races",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Races_Deleted",
                table: "Races",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "IX_Races_Name",
                table: "Races",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Races_Uuid",
                table: "Races",
                column: "Uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Races_WorldId",
                table: "Races",
                column: "WorldId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RaceLanguages");

            migrationBuilder.DropTable(
                name: "Races");
        }
    }
}
