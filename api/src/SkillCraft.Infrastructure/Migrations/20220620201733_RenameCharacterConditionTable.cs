using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillCraft.Infrastructure.Migrations
{
    public partial class RenameCharacterConditionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterCondition_Characters_CharacterId",
                table: "CharacterCondition");

            migrationBuilder.DropForeignKey(
                name: "FK_CharacterCondition_Conditions_ConditionId",
                table: "CharacterCondition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CharacterCondition",
                table: "CharacterCondition");

            migrationBuilder.RenameTable(
                name: "CharacterCondition",
                newName: "CharacterConditions");

            migrationBuilder.RenameIndex(
                name: "IX_CharacterCondition_ConditionId",
                table: "CharacterConditions",
                newName: "IX_CharacterConditions_ConditionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CharacterConditions",
                table: "CharacterConditions",
                columns: new[] { "CharacterId", "ConditionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterConditions_Characters_CharacterId",
                table: "CharacterConditions",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterConditions_Conditions_ConditionId",
                table: "CharacterConditions",
                column: "ConditionId",
                principalTable: "Conditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterConditions_Characters_CharacterId",
                table: "CharacterConditions");

            migrationBuilder.DropForeignKey(
                name: "FK_CharacterConditions_Conditions_ConditionId",
                table: "CharacterConditions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CharacterConditions",
                table: "CharacterConditions");

            migrationBuilder.RenameTable(
                name: "CharacterConditions",
                newName: "CharacterCondition");

            migrationBuilder.RenameIndex(
                name: "IX_CharacterConditions_ConditionId",
                table: "CharacterCondition",
                newName: "IX_CharacterCondition_ConditionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CharacterCondition",
                table: "CharacterCondition",
                columns: new[] { "CharacterId", "ConditionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterCondition_Characters_CharacterId",
                table: "CharacterCondition",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterCondition_Conditions_ConditionId",
                table: "CharacterCondition",
                column: "ConditionId",
                principalTable: "Conditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
