using FastEndpoints;
using Inventory.Prices;
using Microsoft.AspNetCore.Identity;
using Prices.Inventory.Prices;
using System.Reflection;

namespace Inventory.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
           // builder.Services.AddSwaggerGen();
                                  
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
                options.DefaultChallengeScheme = IdentityConstants.BearerScheme;
            }).AddBearerToken(IdentityConstants.BearerScheme);

            builder.Services.AddAuthorizationBuilder();

            List<Assembly> mediatRAssemblies = [typeof(Program).Assembly];

            Users.ConfigureServices.AddServices(builder.Services, builder.Configuration);
         
            Products.ConfigureServices.AddServices(builder.Services,
                builder.Configuration, mediatRAssemblies);
                     
            Prices.ConfigureServices.AddServices(builder.Services, builder.Configuration, mediatRAssemblies);

         //   Prices.RunServices.Run(builder.Services);

            // Set up MediatR
            builder.Services.AddMediatR(cfg =>
              cfg.RegisterServicesFromAssemblies(mediatRAssemblies.ToArray()));

            builder.Services.AddFastEndpoints();

         
            var app = builder.Build();
 
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            
            }
            app.UseFastEndpoints();

            app.MapIdentityApi<IdentityUser>();
            app.Run();
        }
    }
}