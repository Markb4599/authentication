using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authn
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/login";
                    options.AccessDeniedPath = "/denied";
                    options.Events = new CookieAuthenticationEvents()
                    {
                        OnSigningIn = async context =>
                        {
                            var principal = context.Principal;
                            if (principal.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
                            {
                                if(principal.Claims.FirstOrDefault(c =>c.Type==ClaimTypes.NameIdentifier).Value == "bob")
                                {
                                    var.claimsIdentity = principal.Identity as ClaimsIdentity;
                                    ClaimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                                }
                            }

                            await Task.CompletedTask;
                        },
                        OnSignedIn = async context =>
                        {
                            await Task.CompletedTask;
                        }
                    };
                    ///OnValidationPrincipal = async context =>
                    //{
                    //    await Task.CompletedTask;
                    //}
                });
        }
    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            App.UseExceptionHandler("/Home/Error");
            App.UseHsts();
        }
    }
    
        
App.UseHttpRedirection();

App.UseStaticFiles();

App.UseRouting();
App.UseAuthentication();
App.UseAuthorization();

App.UseEndpoints(Endpoints =>
{
    Endpoints.MapControllerRoute(
        name: "default",
        Pattern: "{controller=Home}/{action=Index}/{id?}");
});


}
}
}



namespace WebApplication5_login
{
    public class Startup
    {
    }
}

