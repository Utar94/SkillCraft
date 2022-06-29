START TRANSACTION;

ALTER TABLE "Classes" ALTER COLUMN "OtherRequirements" TYPE text;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220629021924_UpdateClassColumns', '6.0.5');

COMMIT;
