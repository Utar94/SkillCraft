using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SkillCraft.Infrastructure.Migrations
{
    public partial class CreateCharacterTables : Migration
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
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Player = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Aspect1Id = table.Column<int>(type: "integer", nullable: true),
                    Aspect2Id = table.Column<int>(type: "integer", nullable: true),
                    CasteId = table.Column<int>(type: "integer", nullable: true),
                    EducationId = table.Column<int>(type: "integer", nullable: true),
                    NatureId = table.Column<int>(type: "integer", nullable: true),
                    RaceId = table.Column<int>(type: "integer", nullable: true),
                    Stature = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    Weight = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    Age = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Experience = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Vitality = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Stamina = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    BloodAlcoholContent = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Intoxication = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Bonuses = table.Column<string[]>(type: "jsonb[]", nullable: true),
                    Creation = table.Column<string>(type: "jsonb", nullable: true),
                    LevelUps = table.Column<string>(type: "jsonb", nullable: true),
                    SkillRanks = table.Column<string>(type: "jsonb", nullable: true),
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
                name: "CharacterConditions",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    ConditionId = table.Column<int>(type: "integer", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterConditions", x => new { x.CharacterId, x.ConditionId });
                    table.ForeignKey(
                        name: "FK_CharacterConditions_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterConditions_Conditions_ConditionId",
                        column: x => x.ConditionId,
                        principalTable: "Conditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterCustomizations",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    CustomizationId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterCustomizations", x => new { x.CharacterId, x.CustomizationId });
                    table.ForeignKey(
                        name: "FK_CharacterCustomizations_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterCustomizations_Customizations_CustomizationId",
                        column: x => x.CustomizationId,
                        principalTable: "Customizations",
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

            migrationBuilder.CreateTable(
                name: "CharacterTalents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    TalentId = table.Column<int>(type: "integer", nullable: false),
                    OptionId = table.Column<int>(type: "integer", nullable: true),
                    Cost = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterTalents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterTalents_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterTalents_TalentOptions_OptionId",
                        column: x => x.OptionId,
                        principalTable: "TalentOptions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CharacterTalents_Talents_TalentId",
                        column: x => x.TalentId,
                        principalTable: "Talents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterConditions_ConditionId",
                table: "CharacterConditions",
                column: "ConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterCustomizations_CustomizationId",
                table: "CharacterCustomizations",
                column: "CustomizationId");

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

            migrationBuilder.CreateIndex(
                name: "IX_CharacterTalents_CharacterId",
                table: "CharacterTalents",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterTalents_OptionId",
                table: "CharacterTalents",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterTalents_TalentId",
                table: "CharacterTalents",
                column: "TalentId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterTalents_Uuid",
                table: "CharacterTalents",
                column: "Uuid",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterConditions");

            migrationBuilder.DropTable(
                name: "CharacterCustomizations");

            migrationBuilder.DropTable(
                name: "CharacterLanguages");

            migrationBuilder.DropTable(
                name: "CharacterTalents");

            migrationBuilder.DropTable(
                name: "Characters");
        }
    }
}
