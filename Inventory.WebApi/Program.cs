using FastEndpoints;
using Serilog;
using System.Reflection;

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

            List<Assembly> mediatRAssemblies = [typeof(Program).Assembly];
            Products.ConfigureServices.AddServices(builder.Services,
            builder.Configuration, mediatRAssemblies);

            Prices.ConfigureServices.AddServices(builder.Services, builder.Configuration, mediatRAssemblies);
            Inventory.Expressions.ConfigureServices.AddServices(builder.Services, builder.Configuration, mediatRAssemblies);
            Notifications.ConfigureServices.AddServices(builder.Services, builder.Configuration, mediatRAssemblies);
            Transactions.ConfigureServices.AddServices(builder.Services, builder.Configuration, mediatRAssemblies);
            Defaults.ConfigureServices.AddServices(builder.Services, builder.Configuration, mediatRAssemblies);
            builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(mediatRAssemblies.ToArray()));

            // comment on migration run 
            Prices.RunServices.Run(builder.Services);
            Expressions.RunServices.Run(builder.Services);
            Notifications.RunServices.Run(builder.Services);

            builder.Services.AddFastEndpoints();
            var app = builder.Build();


            //comment on migration run
            app.UseSwagger();
            app.UseSwaggerUI((options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            }));

            app.UseFastEndpoints();

            try
            {
                Log.Information("about to app.Run();");
                app.Run();
            }
            catch
            {

            }
        }



    }
}