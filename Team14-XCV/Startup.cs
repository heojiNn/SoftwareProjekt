
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Team14.Data;

using Microsoft.AspNetCore.Components.Authorization;
using BlazorDownloadFile;


namespace Team14
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
            services.AddTransient<IBasicDataSetService, BasicDataSetService>();

            services.AddTransient<ISkillService, SkillService>();
            services.AddTransient<IFieldService, FieldService>();
            services.AddTransient<ILanguageService, LanguageService>();
            services.AddTransient<IRoleService, RoleService>();

            // Nuget: BlazorDownloadFile for Worddownload.
            services.AddBlazorDownloadFile();





        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

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