CREATE TABLE IF NOT EXISTS "EntityFrameworkMigrationHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK_EntityFrameworkMigrationHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
CREATE TABLE "Staffer" (
    "Id" uuid NOT NULL,
    "Email" text NOT NULL,
    "UserId" text,
    "GivenName" text NOT NULL,
    "FamilyName" text NOT NULL,
    "Version" integer NOT NULL,
    CONSTRAINT "PK_Staffer" PRIMARY KEY ("Id")
);

INSERT INTO "EntityFrameworkMigrationHistory" ("MigrationId", "ProductVersion")
VALUES ('20250114011932_InitialCreate', '9.0.0');

COMMIT;

