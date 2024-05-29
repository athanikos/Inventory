using Inventory.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AspNet.Security.OAuth.Validation;

namespace Inventory.WebApi
{
   // https://www.youtube.com/watch?v=S0RSsHKiD6Y


    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            builder.Services.AddSwaggerGen();
                                  
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
                options.DefaultChallengeScheme = IdentityConstants.BearerScheme;
            }).AddBearerToken(IdentityConstants.BearerScheme);

            builder.Services.AddAuthorizationBuilder();
            ConfigureServices.AddServices(builder.Services, builder.Configuration);

            var app = builder.Build();
 
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            
            }


            app.MapIdentityApi<IdentityUser>();

            app.Run();
        }
    }
}