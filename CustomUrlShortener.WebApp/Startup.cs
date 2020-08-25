using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomUrlShortener.DataAccess.DataContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CustomUrlShortener.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CustomUrlShortenerContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("CustomUrlShortenerConnection")));

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, CustomUrlShortenerContext context)
        {
            context.Database.Migrate();

            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Visit",
                    pattern: "{token}",
                    defaults: new { controller = "Url", action = "Visit" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Url}/{action=Index}/{id?}");
            });
        }
    }
}
