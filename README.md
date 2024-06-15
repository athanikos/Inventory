# Inventory


### Products module 
    Manage products. Manage Categories and metrics. 
    Products Categories are just labels. 
    Categories support tree like structure where its node keeps track of its father.
    Each product may have any kind of  metric via the product metric table.  
    Some metric need to have a hard coded type that means something to the system.
    for example QUANTITY should be incremented decremented by transactions. Perhaps need to insert those predefined metric types.
    Souurce represents a system. used to mark a live metric as being updated from some kind of system. For example COINGECKO for crypto values.  

#### Product Metrics example 
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
    



### Prices module 
    
    A hangfire hob that updates productMetric table. 

### Expressions module 

    Supports a simple formula like  METRICCODE(PRODUCTOCODE,UPPERBOUNDEFFECTIVEDATE) where upperbound Effective date.
    It parses metric code , product code and date time and finds the latest product metric row for that perticular date else the latest. 



### Tests Module 

    Runs end to end testing 
    
### Authorization rules 
    
    Authorization rules are applied per user per inventory.
    Claims are Admin (read write on inventory) 
    where claimValue = <InventoryId> and 
    ClaimType = ADMININVENTORY
        
    Client can order from inventory. For an inventory to be able to order 
    claimType = CLIENTINVENTORY claimValue = <InventoryId>

    

## migrations 
    comment out //  builder.Services.AddSwaggerGen(); in startup for the migrations to work
    

    dotnet tool install --global dotnet-ef
    
    cd C:\projects\Inventory\Inventory\Inventory.WebApi
    
    dotnet add package Microsoft.EntityFrameworkCore
    
    dotnet ef migrations add Initial -c  UsersDbContext -p C:\projects\Inventory\Inventory\Inventory.Users\Inventory.Users.csproj -s C:\projects\Inventory\Inventory\Inventory.WebApi\Inventory.WebApi.csproj -o Data/Migrations
    
    
    dotnet ef database update Initial


    C:\projects\Inventory\Inventory\Inventory.WebApi>dotnet ef migrations add Initial -c  ProductsDbContext -p C:\projects\Inventory\Inventory\Inventory.Products\Inventory.Products.csproj -s C:\projects\Inventory\Inventory\Inventory.WebApi\Inventory.WebApi.csproj -o Data/Migrations


    dotnet ef database update Initial



### Prices Module 

    dotnet ef migrations add pPricesv3 -c  PricesDbContext -p C:\projects\Inventory\Inventory\Prices\Inventory.Prices.csproj -s     C:\projects\Inventory\Inventory\Inventory.WebApi\Inventory.WebApi.csproj -o Data/Migrations
    dotnet ef database update  pPricesv3 -c PricesDbContext


  
    
### Metrics 

   One Metric has a source for the value. Source is a source system.
   An example of metric is quantity which has source = SELF where self is the system it self. 
   Transactions affect quantity. 
   A metric for a product is stored in Product Metric table. so the actual values are stored in this table. 




    
