using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SkillCraft.Infrastructure.Migrations
{
    public partial class CreatePowerTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Powers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorldId = table.Column<int>(type: "integer", nullable: false),
                    Tier = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Descriptions = table.Column<string[]>(type: "text[]", nullable: true),
                    Incantation = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Duration = table.Column<int>(type: "integer", nullable: true),
                    Range = table.Column<int>(type: "integer", nullable: true),
                    Ingredients = table.Column<string>(type: "text", nullable: true),
                    Concentration = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Focus = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Ritual = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Somatic = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Verbal = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
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
                    table.PrimaryKey("PK_Powers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Powers_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterPowers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    PowerId = table.Column<int>(type: "integer", nullable: false),
                    Cost = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterPowers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterPowers_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterPowers_Powers_PowerId",
                        column: x => x.PowerId,
                        principalTable: "Powers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterPowers_CharacterId",
                table: "CharacterPowers",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterPowers_PowerId",
                table: "CharacterPowers",
                column: "PowerId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterPowers_Uuid",
                table: "CharacterPowers",
                column: "Uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Powers_CreatedById",
                table: "Powers",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Powers_Deleted",
                table: "Powers",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "IX_Powers_Name",
                table: "Powers",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Powers_Tier",
                table: "Powers",
                column: "Tier");

            migrationBuilder.CreateIndex(
                name: "IX_Powers_Uuid",
                table: "Powers",
                column: "Uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Powers_WorldId",
                table: "Powers",
                column: "WorldId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterPowers");

            migrationBuilder.DropTable(
                name: "Powers");
        }
    }
}
