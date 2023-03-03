using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NucleoRH.Models;

[assembly: HostingStartup(typeof(NucleoRH.Areas.Identity.IdentityHostingStartup))]
namespace NucleoRH.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, opt =>
                {
                    opt.LoginPath = "/Identity/Account/Login";
                    opt.AccessDeniedPath = "/Home/Error";
                });
            });
        }
    }
}