using FastEndpoints;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Identity;
using Serilog;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

namespace Inventory.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

   
            var builder = WebApplication.CreateBuilder(args);

            Log.Information(" WebApplication.CreateBuilder(args)");



            //  comment on migration run
            builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

           
            // https://learn.microsoft.com/en-us/azure/active-directory-b2c/enable-authentication-web-api?tabs=csharpclient
            // Adds Microsoft Identity platform (Azure AD B2C) support to protect this Api
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddMicrosoftIdentityWebApi(options =>
                    {
                        builder.Configuration.Bind("AzureAd", options);

                        options.TokenValidationParameters.NameClaimType = "name";
                    },
            options => { builder.Configuration.Bind("AzureAd", options); });

            //https://www.youtube.com/watch?v=gxPWRq9BteI



            List<Assembly> mediatRAssemblies = [typeof(Program).Assembly];

                Users.ConfigureServices.AddServices(builder.Services, builder.Configuration);

                Products.ConfigureServices.AddServices(builder.Services,
                    builder.Configuration, mediatRAssemblies);

                Prices.ConfigureServices.AddServices(builder.Services, builder.Configuration, mediatRAssemblies);
                Expressions.ConfigureServices.AddServices(builder.Services, builder.Configuration, mediatRAssemblies);
                Notifications.ConfigureServices.AddServices(builder.Services, builder.Configuration, mediatRAssemblies);

                builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblies(mediatRAssemblies.ToArray()));


            // // comment on migration run 
            //Prices.RunServices.Run(builder.Services);
            //Expressions.RunServices.Run(builder.Services);
            //Notifications.RunServices.Run(builder.Services);

            builder.Services.AddFastEndpoints();
                var app = builder.Build();

                if (!app.Environment.IsDevelopment())
                {
                    //comment on migration run
                    app.UseSwagger();
                    app.UseSwaggerUI((options =>
                    {
                        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                        options.RoutePrefix = string.Empty;
                    }));
                }
                app.UseFastEndpoints();

                Log.Information("about to app.Run();");
                app.Run();
         
        }
       
        
       
    }
}