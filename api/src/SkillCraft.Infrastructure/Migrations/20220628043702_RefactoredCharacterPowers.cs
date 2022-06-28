using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SkillCraft.Infrastructure.Migrations
{
    public partial class RefactoredCharacterPowers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CharacterPowers",
                table: "CharacterPowers");

            migrationBuilder.DropIndex(
                name: "IX_CharacterPowers_CharacterId",
                table: "CharacterPowers");

            migrationBuilder.DropIndex(
                name: "IX_CharacterPowers_Uuid",
                table: "CharacterPowers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CharacterPowers");

            migrationBuilder.DropColumn(
                name: "Uuid",
                table: "CharacterPowers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CharacterPowers",
                table: "CharacterPowers",
                columns: new[] { "CharacterId", "PowerId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CharacterPowers",
                table: "CharacterPowers");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CharacterPowers",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<Guid>(
                name: "Uuid",
                table: "CharacterPowers",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CharacterPowers",
                table: "CharacterPowers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterPowers_CharacterId",
                table: "CharacterPowers",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterPowers_Uuid",
                table: "CharacterPowers",
                column: "Uuid",
                unique: true);
        }
    }
}
