using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SkillCraft.Infrastructure.Migrations
{
    public partial class CreateCharacterTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorldId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Player = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Aspect1Id = table.Column<int>(type: "integer", nullable: true),
                    Aspect2Id = table.Column<int>(type: "integer", nullable: true),
                    RaceId = table.Column<int>(type: "integer", nullable: true),
                    NatureId = table.Column<int>(type: "integer", nullable: true),
                    CasteId = table.Column<int>(type: "integer", nullable: true),
                    EducationId = table.Column<int>(type: "integer", nullable: true),
                    Size = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Stature = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    Weight = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    Age = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Experience = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Vitality = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Stamina = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    BloodAlcoholContent = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Intoxication = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Creation = table.Column<string>(type: "jsonb", nullable: true),
                    LevelUps = table.Column<string>(type: "jsonb", nullable: true),
                    SkillRanks = table.Column<string>(type: "jsonb", nullable: true),
                    Bonuses = table.Column<string[]>(type: "jsonb[]", nullable: true),
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
                    table.PrimaryKey("PK_Characters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Characters_Aspects_Aspect1Id",
                        column: x => x.Aspect1Id,
                        principalTable: "Aspects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Characters_Aspects_Aspect2Id",
                        column: x => x.Aspect2Id,
                        principalTable: "Aspects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Characters_Castes_CasteId",
                        column: x => x.CasteId,
                        principalTable: "Castes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Characters_Educations_EducationId",
                        column: x => x.EducationId,
                        principalTable: "Educations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Characters_Natures_NatureId",
                        column: x => x.NatureId,
                        principalTable: "Natures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Characters_Races_RaceId",
                        column: x => x.RaceId,
                        principalTable: "Races",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Characters_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterCondition",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    ConditionId = table.Column<int>(type: "integer", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterCondition", x => new { x.CharacterId, x.ConditionId });
                    table.ForeignKey(
                        name: "FK_CharacterCondition_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterCondition_Conditions_ConditionId",
                        column: x => x.ConditionId,
                        principalTable: "Conditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterLanguages",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterLanguages", x => new { x.CharacterId, x.LanguageId });
                    table.ForeignKey(
                        name: "FK_CharacterLanguages_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterLanguages_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterCondition_ConditionId",
                table: "CharacterCondition",
                column: "ConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterLanguages_LanguageId",
                table: "CharacterLanguages",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_Aspect1Id",
                table: "Characters",
                column: "Aspect1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_Aspect2Id",
                table: "Characters",
                column: "Aspect2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_CasteId",
                table: "Characters",
                column: "CasteId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_CreatedById",
                table: "Characters",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_Deleted",
                table: "Characters",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_EducationId",
                table: "Characters",
                column: "EducationId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_Name",
                table: "Characters",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_NatureId",
                table: "Characters",
                column: "NatureId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_Player",
                table: "Characters",
                column: "Player");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_RaceId",
                table: "Characters",
                column: "RaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_Uuid",
                table: "Characters",
                column: "Uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Characters_WorldId",
                table: "Characters",
                column: "WorldId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterCondition");

            migrationBuilder.DropTable(
                name: "CharacterLanguages");

            migrationBuilder.DropTable(
                name: "Characters");
        }
    }
}
