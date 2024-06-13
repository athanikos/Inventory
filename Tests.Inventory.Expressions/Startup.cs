using Inventory.Products.Repositories;
using Inventory.Products;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace Tests.Inventory.Expressions
{
    /// <summary>
    /// https://github.com/pengweiqhca/Xunit.DependencyInjection
    /// </summary>
    public class Startup 
    {

        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Test.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }
        
        
        public virtual void ConfigureServices(IServiceCollection services)
        {
            List<Assembly> mediatRAssemblies = [typeof(Startup).Assembly];
            services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(mediatRAssemblies.ToArray()));

            services.AddDbContext<ProductsDbContext>(options =>
            options.UseSqlServer(Configuration.
            GetConnectionString("Products")));

            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
        }
        
        
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }
       
    }






}

