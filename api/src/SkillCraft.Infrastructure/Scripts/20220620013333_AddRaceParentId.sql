START TRANSACTION;

ALTER TABLE "Races" ADD "ParentId" integer NULL;

CREATE INDEX "IX_Races_ParentId" ON "Races" ("ParentId");

ALTER TABLE "Races" ADD CONSTRAINT "FK_Races_Races_ParentId" FOREIGN KEY ("ParentId") REFERENCES "Races" ("Id");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220620013333_AddRaceParentId', '6.0.5');

COMMIT;
