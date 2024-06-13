using Inventory.Products.Repositories;
using Inventory.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit.Microsoft.DependencyInjection;

namespace Tests.Inventory.Expressions
{
    /// <summary>
    /// https://umplify.github.io/xunit-dependency-injection/
    /// </summary>
    public  class TestFixture : Xunit.Microsoft.DependencyInjection.Abstracts.TestBedFixture
    {
        protected override void AddServices(IServiceCollection services, IConfiguration? configuration)
        {
            List<Assembly> mediatRAssemblies = [typeof(Startup).Assembly];
            services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(mediatRAssemblies.ToArray()));

            services.AddDbContext<ProductsDbContext>(options =>
            options.UseSqlServer(configuration.
            GetConnectionString("Products")));

            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
        }

        protected override ValueTask DisposeAsyncCore()
        => new();


        protected override IEnumerable<TestAppSettings> GetTestAppSettings()
        {
            yield return new() { Filename = "appsettings.Test.json", IsOptional = false };
        }
    }
}
