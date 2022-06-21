START TRANSACTION;

CREATE TABLE "CharacterCustomizations" (
    "CharacterId" integer NOT NULL,
    "CustomizationId" integer NOT NULL,
    CONSTRAINT "PK_CharacterCustomizations" PRIMARY KEY ("CharacterId", "CustomizationId"),
    CONSTRAINT "FK_CharacterCustomizations_Characters_CharacterId" FOREIGN KEY ("CharacterId") REFERENCES "Characters" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_CharacterCustomizations_Customizations_CustomizationId" FOREIGN KEY ("CustomizationId") REFERENCES "Customizations" ("Id") ON DELETE CASCADE
);

CREATE TABLE "CharacterTalents" (
    "CharacterId" integer NOT NULL,
    "TalentId" integer NOT NULL,
    "Cost" integer NOT NULL DEFAULT 0,
    "Description" text NULL,
    CONSTRAINT "PK_CharacterTalents" PRIMARY KEY ("CharacterId", "TalentId"),
    CONSTRAINT "FK_CharacterTalents_Characters_CharacterId" FOREIGN KEY ("CharacterId") REFERENCES "Characters" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_CharacterTalents_Talents_TalentId" FOREIGN KEY ("TalentId") REFERENCES "Talents" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_CharacterCustomizations_CustomizationId" ON "CharacterCustomizations" ("CustomizationId");

CREATE INDEX "IX_CharacterTalents_TalentId" ON "CharacterTalents" ("TalentId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220621193906_CreateCharacterCustomizationTalentTables', '6.0.5');

COMMIT;
