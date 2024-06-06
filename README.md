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


## Products module 

    One Inventory has many products.
    Many Products have manny categories.
    Many Products have many metrics. 
    
    Authorization rules are applied per user per inventory.
    Claims are Admin (read write on inventory) 
    where claimValue = <InventoryId> and 
    ClaimType = ADMININVENTORY
        
    Client can order from inventory. For an inventory to be able to order 
    claimType = CLIENTINVENTORY claimValue = <InventoryId>

    One Product has some description. A mtric can be acquired value. 
    A product batch was acquired (5 out of toital 20) was acquired at 10 euros but some other batch of 5 items was acquired at 9. 

    ProductId Description  
    1         5

    productId MetricId Quantity Value           EffectiveFrom    
    1         1        5        9               DateMinValue  
    1         1        5        10              DateMinValue       


    MetricId     Description  
    1            AcquiredPrice

    Effective From will allways be min Value in this case since there wil be no change . Acquired also means the inventory bought it increases quantity (?)



    For selling price 
    productId MetricId Quantity Value           EffectiveFrom    
    1         2        10        20              DateMinValue       

    
    MetricId     Description  
    2            SellingPrice
    

    
## Metrics 

   One Metric has a source for the value. Source is a source system.
   An example of metric is quantity which has source = SELF where self is the system it self. 
   Transactions affect quantity. 
   A metric for a product is stored in Product Metric table. so the actual values are stored in this table. 




    
