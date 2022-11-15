using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SolgisFotos.Domain.Services;
using SolgisFotos.Services;

namespace SolgisFotos
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment _env)
        {
            Configuration = configuration;
            root = _env.ContentRootPath;
        }

        public static IConfiguration Configuration { get; set; }

        public static string root { get; set; }
        public static string host { get; set; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddHttpContextAccessor();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireClaim(ClaimTypes.Name);
                });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = "SolmarServer",
                            ValidAudience = "SolmarClients",
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(Configuration["SecurityKey"]))
                        };
                });

            services.AddScoped<IPhotoService, ApiService>();
            services.AddScoped<IPhotoPersonalService, ApiPersonalService>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseExceptionHandler("/error");
            app.UseStaticFiles();

            var rootPath = Path.Combine(root, "Photos");
            var GuiaPath = Path.Combine(rootPath, "Guia");
            var MaterialPath = Path.Combine(rootPath, "Material");
            var PersonalPath = Path.Combine(rootPath, "Personal");


            if (!Directory.Exists(rootPath)) Directory.CreateDirectory(rootPath);
            if (!Directory.Exists(GuiaPath)) Directory.CreateDirectory(GuiaPath);
            if (!Directory.Exists(MaterialPath)) Directory.CreateDirectory(MaterialPath);
            if (!Directory.Exists(PersonalPath)) Directory.CreateDirectory(PersonalPath);

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"Photos")),
                RequestPath = new PathString("/Photos")
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"Photos/Guia")),
                RequestPath = new PathString("/Photos/Guia")
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"Photos/Material")),
                RequestPath = new PathString("/Photos/Material")
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"Photos/Personal")),
                RequestPath = new PathString("/Photos/Personal")
            });

            app.UseHttpsRedirection();
            app.UseMvc();

        }
    }
}
