START TRANSACTION;

ALTER TABLE "Races" RENAME COLUMN "SubraceText" TO "PeopleText";

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220620011622_RenamedSubraceTextToPeopleText', '6.0.5');

COMMIT;
