using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Serilog;
using System.Reflection;

namespace Inventory.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //  comment on migration run
            builder.Services.AddSwaggerGen();

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
            Expressions.ConfigureServices.AddServices(builder.Services, builder.Configuration, mediatRAssemblies);
            Notifications.ConfigureServices.AddServices(builder.Services, builder.Configuration, mediatRAssemblies);

            builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(mediatRAssemblies.ToArray()));


            // comment on migration run 
            Prices.RunServices.Run(builder.Services);
            Expressions.RunServices.Run(builder.Services);
            Notifications.RunServices.Run(builder.Services);

            builder.Services.AddFastEndpoints();
            var app = builder.Build();

            if (app.Environment.IsDevelopment()) {
                // comment on migration run 
                app.UseSwagger();
               app.UseSwaggerUI();
            }
            app.UseFastEndpoints();


           

            app.MapIdentityApi<IdentityUser>();


             Log.Information("about to app.Run();");
             app.Run();
 
         
        }
       
        
       
    }
}