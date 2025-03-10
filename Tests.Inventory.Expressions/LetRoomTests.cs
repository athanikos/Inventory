﻿using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

using Inventory.Products.Contracts;
using Inventory.Products.Contracts.Dto;
using Inventory.Products.Dto;
using Inventory.Products.Repositories;
using Inventory.Products.Services;
using Inventory.Transactions.Repositories;

namespace Tests.Inventory
{
    [Collection("Our Test Collection #1")]
    public class LetRoomTests(ITestOutputHelper testOutputHelper,  
        TestFixture fixture)
        : TestBed<TestFixture>(testOutputHelper, fixture)
    {
        private readonly TestFixture _fixture1 = fixture;

        [Fact]
        public async  Task TestOnInventoryAndRoomsInsert()
        {
            var inventoryRepo = _fixture1.GetService<IInventoryRepository>(_testOutputHelper)!;
            var transactionRepo = _fixture1.GetService<ITransactionRepository>(_testOutputHelper)!;
            inventoryRepo.EmptyDb();
            await transactionRepo.EmptyDb();
            
            var category = (await inventoryRepo.AddCategoryAsync(new CategoryDto(Guid.Empty, "cat",Guid.Empty)));
            Assert.Equal("cat",category.Description);

            
            
            var id =  (await inventoryRepo.AddInventoryAsync(new InventoryDto(Guid.Empty, "TestMe", "test"))).Id;
            var inventory = await inventoryRepo.GetInventoryAsync(id);
            Assert.Equal("TestMe",inventory.Description);
            
            var inventoryService  = _fixture1.GetService<IInventoryService>(_testOutputHelper)!;
            inventoryRepo.EmptyDb();
            await transactionRepo.EmptyDb();
            inventory = await inventoryService.AddInventoryAsync(inventory);
            Assert.Equal("TestMe",inventory.Description);

            var productDto = new ProductDto(Guid.Empty, "Descr", "code", 
                inventory.Id, new List<ProductMetricDto>());
            
            var prod = await inventoryService.AddProductAsync(productDto);
            Assert.Equal("Descr",productDto.Description);
            productDto.Description = "test";
            productDto.Id = prod.Id;
            
            prod = await inventoryService.EditProductAsync(productDto);
            Assert.Equal("test",prod.Description);

            await TestSetup.ClearDb(_testOutputHelper, this._fixture);

        }

        [Fact]
        public void TestOnBuyingTransactionShouldUpdateQuantity()
        {
            Assert.Equal(1,0);
        }

        
        [Fact]
        public void TestOnEnteringRates()
        {
            Assert.Equal(1,0);
        }
        
    }
}
