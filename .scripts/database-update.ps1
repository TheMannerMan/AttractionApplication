# PowerShell script to update the database

# Make sure DbConnection is: SQLServer-musicefc-azkv-docker-sysadmin
# This should be "DbSetActiveIdx": 0 in DbContext/appsettings.json

#If you're running a PowerShell script for the first time, 
# you might need to set the execution policy to allow script execution. In the terminal, run:
#   Set-ExecutionPolicy RemoteSigned -Scope Process


# Update Entity Framework Core tools if needed
dotnet tool update --global dotnet-ef

# Drop any existing database
dotnet ef database drop -f -c SqlServerDbContext -p ../DbContext -s ../DbContext

# Remove any existing migrations
Remove-Item -Recurse -Force ../DbContext/Migrations

# Create a new migration
dotnet ef migrations add miInitial -c SqlServerDbContext -p ../DbContext -s ../DbContext -o ../DbContext/Migrations/SqlServerDbContext

# Update the database from the migration
dotnet ef database update -c SqlServerDbContext -p ../DbContext -s ../DbContext

# Initialize the database (you can run this manually if needed)
# ../DbContext/SqlScripts/initDatabase.sql
# or run ./database-init.ps1
