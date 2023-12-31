﻿START TRANSACTION;

CREATE TABLE "Natures" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "WorldId" integer NOT NULL,
    "Description" text NULL,
    "Name" character varying(256) NOT NULL,
    "Attribute" integer NOT NULL,
    "FeatId" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now()),
    "CreatedById" uuid NOT NULL,
    "Deleted" boolean NOT NULL DEFAULT FALSE,
    "DeletedAt" timestamp with time zone NULL,
    "DeletedById" uuid NULL,
    "UpdatedAt" timestamp with time zone NULL,
    "UpdatedById" uuid NULL,
    "Uuid" uuid NOT NULL DEFAULT (uuid_generate_v4()),
    "Version" integer NOT NULL DEFAULT 0,
    CONSTRAINT "PK_Natures" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Natures_Customizations_FeatId" FOREIGN KEY ("FeatId") REFERENCES "Customizations" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Natures_Worlds_WorldId" FOREIGN KEY ("WorldId") REFERENCES "Worlds" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Natures_CreatedById" ON "Natures" ("CreatedById");

CREATE INDEX "IX_Natures_Deleted" ON "Natures" ("Deleted");

CREATE INDEX "IX_Natures_FeatId" ON "Natures" ("FeatId");

CREATE INDEX "IX_Natures_Name" ON "Natures" ("Name");

CREATE UNIQUE INDEX "IX_Natures_Uuid" ON "Natures" ("Uuid");

CREATE INDEX "IX_Natures_WorldId" ON "Natures" ("WorldId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220609172140_CreateNatureTable', '6.0.5');

COMMIT;
