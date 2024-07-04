﻿using Inventory.Products.Repositories;
using Inventory.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Xunit.Microsoft.DependencyInjection;
using Products = Inventory.Products;
using Inventory.Expressions;

namespace Tests.Inventory.Expressions
{
    /// <summary>
    /// https://umplify.github.io/xunit-dependency-injection/
    /// </summary>
    public  class TestFixture : Xunit.Microsoft.DependencyInjection.Abstracts.TestBedFixture
    {
        protected override void AddServices(IServiceCollection services, IConfiguration? configuration)
        {
            List<Assembly> mediatRAssemblies = [typeof(EvaluatorTests).Assembly];
            mediatRAssemblies.Add(typeof(Products.ConfigureServices).Assembly);
            services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(mediatRAssemblies.ToArray()));

            services.AddEntityFrameworkNpgsql().AddDbContext<ExpressionsDbContext>(options =>
            options.UseNpgsql(configuration.
            GetConnectionString("Expressions")));

            services.AddEntityFrameworkNpgsql().AddDbContext<ProductsDbContext>(options =>
            options.UseNpgsql(configuration.
            GetConnectionString("Products")));

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
