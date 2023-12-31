﻿START TRANSACTION;

CREATE TABLE "Languages" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "WorldId" integer NOT NULL,
    "Description" text NULL,
    "Name" character varying(256) NOT NULL,
    "Exotic" boolean NOT NULL,
    "Script" character varying(256) NULL,
    "TypicalSpeakers" character varying(256) NULL,
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now()),
    "CreatedById" uuid NOT NULL,
    "Deleted" boolean NOT NULL DEFAULT FALSE,
    "DeletedAt" timestamp with time zone NULL,
    "DeletedById" uuid NULL,
    "UpdatedAt" timestamp with time zone NULL,
    "UpdatedById" uuid NULL,
    "Uuid" uuid NOT NULL DEFAULT (uuid_generate_v4()),
    "Version" integer NOT NULL DEFAULT 0,
    CONSTRAINT "PK_Languages" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Languages_Worlds_WorldId" FOREIGN KEY ("WorldId") REFERENCES "Worlds" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Languages_CreatedById" ON "Languages" ("CreatedById");

CREATE INDEX "IX_Languages_Deleted" ON "Languages" ("Deleted");

CREATE INDEX "IX_Languages_Name" ON "Languages" ("Name");

CREATE UNIQUE INDEX "IX_Languages_Uuid" ON "Languages" ("Uuid");

CREATE INDEX "IX_Languages_WorldId" ON "Languages" ("WorldId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220615033306_CreateLanguageTable', '6.0.5');

COMMIT;
