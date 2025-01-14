Migrations: 
	Add: dotnet ef migrations add <MigrationName> -p Company.Infrastructure -s Company.API
	Script: dotnet ef migrations script -p Company.Infrastructure -s Company.API -o EFMigrationScript.sql
	Update: dotnet ef database update -p Company.Infrastructure -s Company.API
	Remove: dotnet ef migrations remove -p Company.Infrastructure -s Company.API