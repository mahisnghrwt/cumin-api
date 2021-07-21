using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cumin_api.Middlewares;
using cumin_api.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using cumin_api.Attributes;

namespace cumin_api {
    public class Startup {
        private readonly string AllowSpecificOriginCorsPolicy = "AllowSpecificOriginCorsPolicy";
        private readonly string FrontendUrl = "http://localhost:3000";
        private readonly string RTDBUrl = "http://localhost:21941";
        private readonly string RTDBUrlSSL = "http://localhost:44332";

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddCors(options =>
                options.AddPolicy(AllowSpecificOriginCorsPolicy, builder => {
                    builder.WithOrigins(new[] { FrontendUrl, RTDBUrl, RTDBUrlSSL }).AllowAnyHeader().AllowAnyMethod().AllowCredentials();

                })
            );

            services.AddControllers();

            services.Configure<SecurityConfiguration>(Configuration.GetSection("Security"));
            
            services.AddDbContext<CuminApiContext>(options => options.UseMySql(Configuration.GetConnectionString("Default"), new MySqlServerVersion(new Version(8, 0, 25))));

            services.AddScoped<IIssueService, IssueService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISprintService, SprintService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddSingleton<IWebSocketService, WebSocketService>();
            services.AddScoped<RealtimeRequestFilter, RealtimeRequestFilter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(AllowSpecificOriginCorsPolicy);

            app.UseAuthorization();

            app.UseMiddleware<JwtTokenMiddleware>(); 

            // app.UseMiddleware<ProjectAuthorizationMiddleware>();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
