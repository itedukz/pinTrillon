add-migration AddProductTables


update-database


Remove-Migration

dotnet user-secrets set "Authentication:Google:ClientId" "<client-id>"
dotnet user-secrets set "Authentication:Google:ClientSecret" "<client-secret>"

hide the db connection string