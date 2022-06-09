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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorldId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    MandatoryAttribute1 = table.Column<int>(type: "integer", nullable: false),
                    MandatoryAttribute2 = table.Column<int>(type: "integer", nullable: false),
                    OptionalAttribute1 = table.Column<int>(type: "integer", nullable: false),
                    OptionalAttribute2 = table.Column<int>(type: "integer", nullable: false),
                    Skill1 = table.Column<int>(type: "integer", nullable: false),
                    Skill2 = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Aspects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aspects_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aspects_CreatedById",
                table: "Aspects",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Aspects_Deleted",
                table: "Aspects",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "IX_Aspects_Name",
                table: "Aspects",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Aspects_Uuid",
                table: "Aspects",
                column: "Uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Aspects_WorldId",
                table: "Aspects",
                column: "WorldId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aspects");
        }
    }
}
