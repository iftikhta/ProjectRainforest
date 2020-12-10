using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectRainforest.Areas.Identity.Data;
using ProjectRainforest.Data;

[assembly: HostingStartup(typeof(ProjectRainforest.Areas.Identity.IdentityHostingStartup))]
namespace ProjectRainforest.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<RainforestAuthContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("azureRainforestDB")));

                services.AddDefaultIdentity<RainforestUser>(options => {

                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    
                    })
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<RainforestAuthContext>();
            });
        }
    }
}