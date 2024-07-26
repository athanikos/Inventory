using Inventory.BlazorApp.Components;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web.UI;
using Microsoft.Identity.Web;
using System.Reflection;
using System.Security.Claims;
using System.Linq.Dynamic.Core;


namespace Inventory.BlazorApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Read the connection string from the appsettings.json file
            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            // Get HostingEnvironment
            var env = builder.Environment;
            builder.Configuration.AddJsonFile($"appsettings{env.EnvironmentName}.json", optional: true);
            builder.Configuration.AddEnvironmentVariables()
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true);
            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddMicrosoftIdentityConsentHandler();
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddHttpClient();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<HttpContextAccessor>();
            // This is where you wire up to events to detect when a user Log in
            builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(options =>
                {
                    builder.Configuration.Bind("AzureAdB2C", options);
                    options.Events = new OpenIdConnectEvents
                    {
                        OnRedirectToIdentityProvider = async ctxt =>
                        {
                            // Invoked before redirecting to the identity provider to authenticate. 
                            // This can be used to set ProtocolMessage.State
                            // that will be persisted through the authentication process. 
                            // The ProtocolMessage can also be used to add or customize
                            // parameters sent to the identity provider.
                            await Task.Yield();
                        },
                        OnAuthenticationFailed = async ctxt =>
                        {
                            // They tried to log in but it failed
                            await Task.Yield();
                        },
                        OnSignedOutCallbackRedirect = async ctxt =>
                        {
                            ctxt.HttpContext.Response.Redirect(ctxt.Options.SignedOutRedirectUri);
                            ctxt.HandleResponse();
                            await Task.Yield();
                        },
                        OnTicketReceived = async ctxt =>
                        {
                            if (ctxt.Principal != null)
                            {
                                if (ctxt.Principal.Identity is ClaimsIdentity identity)
                                {
                                    var colClaims = await ctxt.Principal.Claims.ToDynamicListAsync();
                                    var IdentityProvider = colClaims.FirstOrDefault(
                                        c => c.Type == "http://schemas.microsoft.com/identity/claims/identityprovider")?.Value;
                                    var Objectidentifier = colClaims.FirstOrDefault(
                                        c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
                                    var EmailAddress = colClaims.FirstOrDefault(
                                        c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
                                    var FirstName = colClaims.FirstOrDefault(
                                        c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")?.Value;
                                    var LastName = colClaims.FirstOrDefault(
                                        c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname")?.Value;
                                    var AzureB2CFlow = colClaims.FirstOrDefault(
                                        c => c.Type == "http://schemas.microsoft.com/claims/authnclassreference")?.Value;
                                    var auth_time = colClaims.FirstOrDefault(
                                        c => c.Type == "auth_time")?.Value;
                                    var DisplayName = colClaims.FirstOrDefault(
                                        c => c.Type == "name")?.Value;
                                    var idp_access_token = colClaims.FirstOrDefault(
                                        c => c.Type == "idp_access_token")?.Value;
                                }
                            }
                            await Task.Yield();
                        },
                    };
                });
            builder.Services.AddControllersWithViews()
                .AddMicrosoftIdentityUI();
            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.UseAntiforgery();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();
            app.Run();
        }
    }
}
