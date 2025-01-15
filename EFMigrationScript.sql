CREATE TABLE IF NOT EXISTS "EntityFrameworkMigrationHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK_EntityFrameworkMigrationHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
CREATE TABLE "Company" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "Address_Address1" text,
    "Address_Address2" text,
    "Address_City" text,
    "Address_StateProvince" text,
    "Address_Country" text,
    "Address_PostalCode" text,
    "Version" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "ModifiedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Company" PRIMARY KEY ("Id")
);

CREATE TABLE "Staffer" (
    "Id" uuid NOT NULL,
    "CompanyId" uuid NOT NULL,
    "Email" text NOT NULL,
    "UserId" text,
    "GivenName" text NOT NULL,
    "FamilyName" text NOT NULL,
    "IsOwner" boolean NOT NULL,
    "Version" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "ModifiedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Staffer" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Staffer_Company_CompanyId" FOREIGN KEY ("CompanyId") REFERENCES "Company" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Staffer_CompanyId" ON "Staffer" ("CompanyId");

INSERT INTO "EntityFrameworkMigrationHistory" ("MigrationId", "ProductVersion")
VALUES ('20250115192535_InitialCreate', '9.0.0');

COMMIT;

