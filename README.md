Migrations: 
	Add: dotnet ef migrations add <MigrationName> -p WireOps.Infrastructure -s WireOps.API
	Script: dotnet ef migrations script -p WireOps.Infrastructure -s WireOps.API -o EFMigrationScript.sql
	Update: dotnet ef database update -p WireOps.Infrastructure -s WireOps.API
	Remove: dotnet ef migrations remove -p WireOps.Infrastructure -s WireOps.API