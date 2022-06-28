START TRANSACTION;

ALTER TABLE "CharacterPowers" DROP CONSTRAINT "PK_CharacterPowers";

DROP INDEX "IX_CharacterPowers_CharacterId";

DROP INDEX "IX_CharacterPowers_Uuid";

ALTER TABLE "CharacterPowers" DROP COLUMN "Id";

ALTER TABLE "CharacterPowers" DROP COLUMN "Uuid";

ALTER TABLE "CharacterPowers" ADD CONSTRAINT "PK_CharacterPowers" PRIMARY KEY ("CharacterId", "PowerId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220628043702_RefactoredCharacterPowers', '6.0.5');

COMMIT;
