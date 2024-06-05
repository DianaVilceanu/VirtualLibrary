using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GameLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using GameLibrary.Repository;
using Microsoft.AspNetCore.Http;


namespace GameLibrary
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddDbContext<StoreContext>(
                 options => options.UseNpgsql(System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != null
                 && System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production" ?
                 System.Environment.GetEnvironmentVariable("CONNECTIONSTRING") : Configuration.GetConnectionString("PostgreSQL"), x => x.MigrationsHistoryTable("__MigrationHistory", "Store")));
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<StoreContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            


            services.AddTransient<IBookRepository, BookRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();

            //!!!!!!!
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(sp => ShoppingCart.GetCart(sp));
            services.AddScoped(sp => SCart.GetCart(sp));
            services.AddMvc();
            services.AddMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            services.AddSession();


            //MvcOptions.EnableEndpointRouting = false;
            //

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // IMPORTANT: This session call MUST go before UseMvc()
            app.UseSession();

          

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
        private void ConfigureRoute(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
