# Inventory



## installing migrations 
    comment out //  builder.Services.AddSwaggerGen(); in startup for the migrations to work
    

    dotnet tool install --global dotnet-ef
    
    cd C:\projects\Inventory\Inventory\Inventory.WebApi
    
    dotnet add package Microsoft.EntityFrameworkCore
    
    dotnet ef migrations add Initial -c  UsersDbContext -p C:\projects\Inventory\Inventory\Inventory.Users\Inventory.Users.csproj -s C:\projects\Inventory\Inventory\Inventory.WebApi\Inventory.WebApi.csproj -o Data/Migrations
    
    
    dotnet ef database update Initial


C:\projects\Inventory\Inventory\Inventory.WebApi>dotnet ef migrations add Initial -c  ProductsDbContext -p C:\projects\Inventory\Inventory\Inventory.Products\Inventory.Products.csproj -s C:\projects\Inventory\Inventory\Inventory.WebApi\Inventory.WebApi.csproj -o Data/Migrations


    dotnet ef database update Initial



