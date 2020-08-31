using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Student2020.Configs;
using Student2020.Handler;
using Student2020.Models;

namespace Student2020
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
            services.AddDbContext<NhapHoc2020Context>(opt =>
               opt.UseSqlServer(Configuration.GetSection("StrConnection").Value));
            services.AddControllers();
            services.AddMvc(ConfigOptions);
            services.AddCors();

            var appConfig = new AppConfig();
            Configuration.Bind(nameof(AppConfig), appConfig);
            services.AddSingleton(appConfig);

            if (!Directory.Exists(appConfig.DocumentPath))
            {
                Directory.CreateDirectory(appConfig.DocumentPath);
            }

            if (!Directory.Exists(appConfig.ImagePath))
            {
                Directory.CreateDirectory(appConfig.ImagePath);
            }
        }

        private static void ConfigOptions(MvcOptions option)
        {
            option.Filters.Add(typeof(BadRequestExceptionHandler));
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
               // app.UseDeveloperExceptionPage();
            }
            

            app.UseHttpsRedirection();

            var options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(options);
            app.UseRouting();
            app.UseStaticFiles();
            app.UseAuthorization();
            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:5001");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
