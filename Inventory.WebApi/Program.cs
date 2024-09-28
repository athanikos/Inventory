using FastEndpoints;
using Serilog;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

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
            
            var key = "_9T8Q~n6BCl3fhmXpMWXZxJvov4tNMeT4LgzxbpO";
            SymmetricSecurityKey signingKey = 
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key.PadRight((512/8), '\0')));
                
            // https://learn.microsoft.com/en-us/azure/active-directory-b2c/enable-authentication-web-api?tabs=csharpclient
            // Adds Microsoft Identity platform (Azure AD B2C) support to protect this Api
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddMicrosoftIdentityWebApi(options =>
            {
                        builder.Configuration.Bind("AzureAd", options);
                        options.IncludeErrorDetails = true;
                        options.TokenValidationParameters.NameClaimType = "name";
              
                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateAudience = true,
                            ValidateIssuer = true,
                            ValidIssuer = "https://login.microsoftonline.com/1de0141d-ab7b-4039-b464-c45ca4cf4b04/v2.0",
                            ValidAudience =  "5ad600b2-d388-4c0a-84b1-a976ab9691f9",
                            RequireSignedTokens = true,
                            IssuerSigningKey = signingKey,
                            ValidateLifetime = true
                        };
           },
            options => { builder.Configuration.Bind("AzureAd", options); });

           List<Assembly> mediatRAssemblies = [typeof(Program).Assembly];
           Products.ConfigureServices.AddServices(builder.Services,
           builder.Configuration, mediatRAssemblies);
            
           Prices.ConfigureServices.AddServices(builder.Services, builder.Configuration, mediatRAssemblies);
           Expressions.ConfigureServices.AddServices(builder.Services, builder.Configuration, mediatRAssemblies);
           Notifications.ConfigureServices.AddServices(builder.Services, builder.Configuration, mediatRAssemblies);
           Transactions.ConfigureServices.AddServices(builder.Services, builder.Configuration, mediatRAssemblies);
           Defaults.ConfigureServices.AddServices(builder.Services, builder.Configuration, mediatRAssemblies);
           builder.Services.AddMediatR(cfg =>
           cfg.RegisterServicesFromAssemblies(mediatRAssemblies.ToArray()));

           // comment on migration run 
           //Prices.RunServices.Run(builder.Services);
           //Expressions.RunServices.Run(builder.Services);
           //Notifications.RunServices.Run(builder.Services);

           builder.Services.AddFastEndpoints();
           var app = builder.Build();
           app.UseAuthentication(); 
           app.UseAuthorization(); 
           IdentityModelEventSource.ShowPII = true; 
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