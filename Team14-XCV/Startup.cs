using System;
using System.Threading;
using BlazorDownloadFile;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XCV.Data;


namespace XCV
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddTransient<AuthenticationStateProvider, CustomAuthentiProvider>();
            services.AddTransient<CustomAuthentiProvider>();
            services.AddTransient<IAccountService, EmployeeService>();

            services.AddTransient<IProfileService, EmployeeService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<IBasicDataSetService, BasicDataSetService>();

            services.AddTransient<IOfferService, OfferService>();
            services.AddTransient<IConfigService, ConfigService>();
            services.AddTransient<IGenerateService, GenerateService>();
            services.AddSingleton<OfferData>();

            services.AddSingleton<ISkillService, SkillService>();
            services.AddTransient<IFieldService, FieldService>();
            services.AddTransient<ILanguageService, LanguageService>();
            services.AddTransient<IRoleService, RoleService>();

            services.AddTransient<DatabaseUtils>();
            services.AddTransient<InsertRandomData>();
            // Nuget: BlazorDownloadFile for Worddownload.
            services.AddBlazorDownloadFile();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DatabaseUtils dbu, IServiceProvider sp)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            dbu.CreateDatabase();
            dbu.CreateTables();

            var ird = sp.GetService<InsertRandomData>();
            ird.InsertJson();
            Thread.Sleep(1000);
            ird.Insert6Employyes();


            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
