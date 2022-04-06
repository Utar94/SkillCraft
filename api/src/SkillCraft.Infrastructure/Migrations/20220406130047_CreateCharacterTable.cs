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
                    Description = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Player = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    WorldId = table.Column<int>(type: "integer", nullable: false),
                    Stature = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    Weight = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    Age = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Experience = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Vitality = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Stamina = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    BloodAlcoholContent = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Intoxication = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedById = table.Column<Guid>(type: "uuid", nullable: true),
                    Key = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Characters_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Characters_CreatedById",
                table: "Characters",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_Deleted",
                table: "Characters",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_Key",
                table: "Characters",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Characters_Name",
                table: "Characters",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_WorldId",
                table: "Characters",
                column: "WorldId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Characters");
        }
    }
}
