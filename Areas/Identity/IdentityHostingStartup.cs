using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniversityManagementApp.Areas.Identity.Data;
using UniversityManagementApp.Data;

[assembly: HostingStartup(typeof(UniversityManagementApp.Areas.Identity.IdentityHostingStartup))]
namespace UniversityManagementApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<UniversityManagementAppContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("UniversityManagementAppContext")));
            });
        }
    }
}