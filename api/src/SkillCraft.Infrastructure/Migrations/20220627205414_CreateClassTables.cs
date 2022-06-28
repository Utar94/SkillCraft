using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SkillCraft.Infrastructure.Migrations
{
    public partial class CreateClassTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorldId = table.Column<int>(type: "integer", nullable: false),
                    UniqueTalentId = table.Column<int>(type: "integer", nullable: false),
                    Tier = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    OtherRequirements = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    OtherTalentsText = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
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
                    table.PrimaryKey("PK_Classes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classes_Talents_UniqueTalentId",
                        column: x => x.UniqueTalentId,
                        principalTable: "Talents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Classes_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassTalents",
                columns: table => new
                {
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    TalentId = table.Column<int>(type: "integer", nullable: false),
                    Mandatory = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassTalents", x => new { x.ClassId, x.TalentId });
                    table.ForeignKey(
                        name: "FK_ClassTalents_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassTalents_Talents_TalentId",
                        column: x => x.TalentId,
                        principalTable: "Talents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classes_CreatedById",
                table: "Classes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_Deleted",
                table: "Classes",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_Name",
                table: "Classes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_Tier",
                table: "Classes",
                column: "Tier");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_UniqueTalentId",
                table: "Classes",
                column: "UniqueTalentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_Uuid",
                table: "Classes",
                column: "Uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_WorldId",
                table: "Classes",
                column: "WorldId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassTalents_TalentId",
                table: "ClassTalents",
                column: "TalentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassTalents");

            migrationBuilder.DropTable(
                name: "Classes");
        }
    }
}
