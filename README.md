# Inventory
A generic Inventory Implementation that allows:
Buy/ Sell / Let or EndLet properties, Those add or subtract from Quantity Metric table.
Template Creation for any type of entity. Currently supported for transactions. 
Dynamic metrics creation via ProductMetric table. 
Create formulas for any type of metric. 
Notify on threshold. 

### Transactions Service 

Inserts, Updates Transactions. Gets Default new Transaction from template.

### QuantityModifyService
Supports Add/Subtract through buy/sell and let through quantity metric table. 
Supports cancelling entries.
Chain updates quantity post effective date. 


### Expressions Service
Product Based Formula
Inventory Based Formula 
Boolean Formulas 

### Notifications Service 

TBD 


### Configuration
Holds the default values for entities. 
Configures the initial state. 


#### LivePricesService

TBD 






### Availability Service 


### Rates Service 






### Quantity via QuantityMetric    

### Products module 
Manage products. Manage Categories and metrics. 
Products Categories are just labels. 
Categories support tree like structure where its node keeps track of its father. Each product may have any kind of  metric via the product metric table.  
Perhaps need to insert those predefined metric types. Source represents a system. used to mark a live metric as being updated from some kind of system. 


#### Product Metrics example 
One Product has some description. A metric can be acquired value. 
A product batch was acquired (5 out of toital 20) at 10 euros but some other batch of 5 items was acquired at 9. 

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
    
A hangfire job that retrieves prices from an external endpoint and updates prices in productMetric table. 




### Expressions module

Uses Ncalc for evaluation. 

Evaluates three types of expressions:
    Boolean Expressions ie RPICE([ADA]) > 1. Saves the result of the expression to Notification table in notifications table. 
    Product Expressions ie these are expressions that compute some attribute for a product . example PRICE([ADA]) * VALUE([ADA]) 
    Inventory Expressions. computes some attribute per inventory . Example SUM(VALUE([ALL])) 


### Authorization rules 
    
Authorization rules are applied per user per inventory.
Claims are Admin (read write on inventory) 
where claimValue = <InventoryId> and 
ClaimType = ADMININVENTORY
        
Client can order from inventory. For an inventory to be able to order 
ClaimType = CLIENTINVENTORY claimValue = <InventoryId>
   

## migrations (postgres sql )
    
    cd C:\projects\Inventory\Inventory\Inventory.WebApi
    
    
    dotnet ef migrations add PG -c  UsersDbContext -p C:\projects\Inventory\Inventory\Inventory.Users\Inventory.Users.csproj -s C:\projects\Inventory\Inventory\Inventory.WebApi\Inventory.WebApi.csproj -o Data/Migrations
    dotnet ef database update PG -c  UsersDbContext -p C:\projects\Inventory\Inventory\Inventory.Users\Inventory.Users.csproj 
    
    dotnet ef migrations add PG -c  ProductsDbContext -p C:\projects\Inventory\Inventory\Inventory.Products\Inventory.Products.csproj -s C:\projects\Inventory\Inventory\Inventory.WebApi\Inventory.WebApi.csproj -o Data/Migrations
    dotnet ef database update PG  -c  ProductsDbContext -p C:\projects\Inventory\Inventory\Inventory.Products\Inventory.Products.csproj 
    
    
    
    dotnet ef migrations add PG  -c  PricesDbContext -p C:\projects\Inventory\Inventory\Prices\Inventory.Prices.csproj -s     C:\projects\Inventory\Inventory\Inventory.WebApi\Inventory.WebApi.csproj -o Data/Migrations
    
    dotnet ef database update  pPricesv3 -c PricesDbContext
    
    
    dotnet ef migrations add PG -c  ExpressionsDbContext -p C:\projects\Inventory\Inventory\Expressions\Inventory.Expressions.csproj -s C:\projects\Inventory\Inventory\Inventory.WebApi\Inventory.WebApi.csproj -o Data/Migrations
    
    
    dotnet ef database update   PG -c ExpressionsDbContext
    
    
    dotnet ef migrations add PG  -c NotifierDbContext -p C:\projects\Inventory\Inventory\Inventory.Notifier\Inventory.Notifications.csproj -s  C:\projects\Inventory\Inventory\Inventory.WebApi\Inventory.WebApi.csproj -o Data/Migrations
    
    dotnet ef database update   PG -c NotifierDbContext

    dotnet ef migrations add PG7  -c TransactionsDbContext -p C:\projects\Inventory\Inventory\Inventory.Transactions\Inventory.Transactions.csproj  -s  C:\projects\Inventory\Inventory\Inventory.WebApi\Inventory.WebApi.csproj -o Data/Migrations
    dotnet ef database update   PG7 -c TransactionsDbContext

    dotnet ef migrations add PG7  -c ConfigurationDbContext -p C:\projects\Inventory\Inventory\Inventory.Configuration\Inventory.Configuration.csproj  -s  C:\projects\Inventory\Inventory\Inventory.WebApi\Inventory.WebApi.csproj -o Data/Migrations
    dotnet ef database update   PG7 -c ConfigurationDbContext

### Metrics 

   One Metric has a source for the value. Source is a source system.
   An example of metric is quantity which has source = SELF where self is the system it self. 
   Transactions affect quantity. 
   A metric for a product is stored in Product Metric table. so the actual values are stored in this table. 



### Use cases

#### Read Invoices module (extract text and insert) 

#### Notify on threshold 

Set a threshold and notify
        
#### Compute total portofolio value & display   
Sum all balances 

#### any product pricing through ebay ?

#### Betting portofolio support 

### Rooms to let 

Stores Room as product.
Update Rooms quantity via Transaction. 
Allows Let via Transaction.



Room management 

Each room is a product. Rooms are added via product management.
All rooms are created under an inventoryId.
To add quantity an init transaction that buys a room should be created.


Rates 

A let price per day is added via Product Metric table.
A let action can happen via transactions service. 


#### invoice items 

dynamic fields 
for rooms it has line items :

quantity - entered and validated (days)
price per day - computed by product metrics 
discount value - enterwd 
datefrom inclusive 
dateto inclusive 




    
