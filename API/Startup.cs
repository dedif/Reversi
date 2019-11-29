using System;
using System.IO;
using API.DAL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API
{
    /// <summary>
    /// The startup class that contains init methods
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// The constructor that takes in the configuration object
        /// </summary>
        /// <param name="configuration">The configuration object</param>
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// The ASP.NET Core Environment
        /// </summary>
        public IHostingEnvironment HostingEnvironment { get; set; }

        /// <summary>
        /// Name of the CORS-policy that will allow Browser-sync while debugging
        /// </summary>
        private const string AllowBrowserSync = "_allowBrowserSync";

        /// <summary>
        /// The configuration object
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Where the wwwroot directory is
        /// </summary>
        public static string WebRootPath { get; set; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The services that need to be configured</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            if (HostingEnvironment.IsDevelopment())
            {
                services.AddCors(options =>
                {
                    options.AddPolicy(AllowBrowserSync,
                        builder =>
                        {
                            builder.WithOrigins("http://localhost:3000");
                        });
                });
            }
            services.AddSingleton<IPlayerDal, PlayerDal>();
            services.AddSingleton<IGameDal, GameDal>();
            services.AddSingleton<IPlayerGameDal, PlayerGameDal>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application builder</param>
        /// <param name="env">The hosting environment</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            if (env.IsDevelopment())
            {
                app.UseCors(AllowBrowserSync);
            }
            //app.UseCors("AllowAll");
            //app.UseCors(AllowBrowserSync);
            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            WebRootPath = env.WebRootPath;
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMvc();


#if DEBUG
            try
            {
                File.WriteAllText("browsersync-update.txt", DateTime.Now.ToString());
            }
            catch
            {
                // ignore
            }
#endif
        }
    }
}
