START TRANSACTION;

ALTER TABLE "Talents" ADD "Skill" integer NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220621171712_AddSkillToTalentTable', '6.0.5');

COMMIT;
