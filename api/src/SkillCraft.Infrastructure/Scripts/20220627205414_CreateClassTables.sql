﻿START TRANSACTION;

CREATE TABLE "Classes" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "WorldId" integer NOT NULL,
    "UniqueTalentId" integer NOT NULL,
    "Tier" integer NOT NULL,
    "Name" character varying(256) NOT NULL,
    "Description" text NULL,
    "OtherRequirements" character varying(256) NULL,
    "OtherTalentsText" character varying(256) NULL,
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now()),
    "CreatedById" uuid NOT NULL,
    "Deleted" boolean NOT NULL DEFAULT FALSE,
    "DeletedAt" timestamp with time zone NULL,
    "DeletedById" uuid NULL,
    "UpdatedAt" timestamp with time zone NULL,
    "UpdatedById" uuid NULL,
    "Uuid" uuid NOT NULL DEFAULT (uuid_generate_v4()),
    "Version" integer NOT NULL DEFAULT 0,
    CONSTRAINT "PK_Classes" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Classes_Talents_UniqueTalentId" FOREIGN KEY ("UniqueTalentId") REFERENCES "Talents" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Classes_Worlds_WorldId" FOREIGN KEY ("WorldId") REFERENCES "Worlds" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ClassTalents" (
    "ClassId" integer NOT NULL,
    "TalentId" integer NOT NULL,
    "Mandatory" boolean NOT NULL DEFAULT FALSE,
    CONSTRAINT "PK_ClassTalents" PRIMARY KEY ("ClassId", "TalentId"),
    CONSTRAINT "FK_ClassTalents_Classes_ClassId" FOREIGN KEY ("ClassId") REFERENCES "Classes" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_ClassTalents_Talents_TalentId" FOREIGN KEY ("TalentId") REFERENCES "Talents" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Classes_CreatedById" ON "Classes" ("CreatedById");

CREATE INDEX "IX_Classes_Deleted" ON "Classes" ("Deleted");

CREATE INDEX "IX_Classes_Name" ON "Classes" ("Name");

CREATE INDEX "IX_Classes_Tier" ON "Classes" ("Tier");

CREATE UNIQUE INDEX "IX_Classes_UniqueTalentId" ON "Classes" ("UniqueTalentId");

CREATE UNIQUE INDEX "IX_Classes_Uuid" ON "Classes" ("Uuid");

CREATE INDEX "IX_Classes_WorldId" ON "Classes" ("WorldId");

CREATE INDEX "IX_ClassTalents_TalentId" ON "ClassTalents" ("TalentId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220627205414_CreateClassTables', '6.0.5');

COMMIT;
