Migrations: 
	Add: dotnet ef migrations add <MigrationName> -p Business.Infrastructure -s Business.API
	Script: dotnet ef migrations script -p Business.Infrastructure -s Business.API -o EFMigrationScript.sql
	Update: dotnet ef database update -p Business.Infrastructure -s Business.API
	Remove: dotnet ef migrations remove -p Business.Infrastructure -s Business.API