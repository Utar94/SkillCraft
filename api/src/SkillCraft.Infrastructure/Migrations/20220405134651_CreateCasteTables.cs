using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SkillCraft.Infrastructure.Migrations
{
    public partial class CreateCasteTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Castes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Skill = table.Column<int>(type: "integer", nullable: true),
                    WealthRoll = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    WorldId = table.Column<int>(type: "integer", nullable: true),
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
                    table.PrimaryKey("PK_Castes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Castes_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CasteTraits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CasteId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Key = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CasteTraits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CasteTraits_Castes_CasteId",
                        column: x => x.CasteId,
                        principalTable: "Castes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Castes_CreatedById",
                table: "Castes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Castes_Deleted",
                table: "Castes",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "IX_Castes_Key",
                table: "Castes",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Castes_Name",
                table: "Castes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Castes_WorldId",
                table: "Castes",
                column: "WorldId");

            migrationBuilder.CreateIndex(
                name: "IX_CasteTraits_CasteId",
                table: "CasteTraits",
                column: "CasteId");

            migrationBuilder.CreateIndex(
                name: "IX_CasteTraits_Key",
                table: "CasteTraits",
                column: "Key",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CasteTraits");

            migrationBuilder.DropTable(
                name: "Castes");
        }
    }
}
