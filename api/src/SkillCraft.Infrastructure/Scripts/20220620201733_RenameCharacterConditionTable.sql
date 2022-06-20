START TRANSACTION;

ALTER TABLE "CharacterCondition" DROP CONSTRAINT "FK_CharacterCondition_Characters_CharacterId";

ALTER TABLE "CharacterCondition" DROP CONSTRAINT "FK_CharacterCondition_Conditions_ConditionId";

ALTER TABLE "CharacterCondition" DROP CONSTRAINT "PK_CharacterCondition";

ALTER TABLE "CharacterCondition" RENAME TO "CharacterConditions";

ALTER INDEX "IX_CharacterCondition_ConditionId" RENAME TO "IX_CharacterConditions_ConditionId";

ALTER TABLE "CharacterConditions" ADD CONSTRAINT "PK_CharacterConditions" PRIMARY KEY ("CharacterId", "ConditionId");

ALTER TABLE "CharacterConditions" ADD CONSTRAINT "FK_CharacterConditions_Characters_CharacterId" FOREIGN KEY ("CharacterId") REFERENCES "Characters" ("Id") ON DELETE CASCADE;

ALTER TABLE "CharacterConditions" ADD CONSTRAINT "FK_CharacterConditions_Conditions_ConditionId" FOREIGN KEY ("ConditionId") REFERENCES "Conditions" ("Id") ON DELETE CASCADE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220620201733_RenameCharacterConditionTable', '6.0.5');

COMMIT;
