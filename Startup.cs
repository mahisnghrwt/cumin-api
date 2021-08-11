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
using cumin_api.Others;
using System.Net;
using Microsoft.AspNetCore.HttpOverrides;

namespace cumin_api {
    public class Startup {
        private readonly string AllowSpecificOriginCorsPolicy = "AllowSpecificOriginCorsPolicy";
        private readonly string FrontendUrl = "http://localhost:3000";
        //private readonly string RTDBUrl = "http://localhost:21941";
        //private readonly string RTDBUrlSSL = "http://localhost:44332";

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            // added
            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = (int)HttpStatusCode.PermanentRedirect;
                options.HttpsPort = 5001;
            });


            services.AddCors(options =>
                options.AddPolicy(AllowSpecificOriginCorsPolicy, builder => {
                    //builder.WithOrigins(new[] { FrontendUrl, RTDBUrl, RTDBUrlSSL }).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                    builder.WithOrigins(new[] { FrontendUrl }).AllowAnyHeader().AllowAnyMethod().AllowCredentials();

                })
            );

            services.AddControllers();

            services.Configure<SecurityConfiguration>(Configuration.GetSection("Security"));
            
            services.AddDbContext<CuminApiContext>(options => options.UseMySql(Configuration.GetConnectionString("Default"), new MySqlServerVersion(new Version(8, 0, 25))));
            services.AddSingleton<StateManager, StateManager>();
            services.AddScoped<TokenHelper, TokenHelper>();
            services.AddScoped<SockService, SockService>();

            // Db Services
            services.AddScoped<Services.v2.IssueService, Services.v2.IssueService>();
            services.AddScoped<Services.v2.UserService, Services.v2.UserService>();
            services.AddScoped<Services.v2.SprintService, Services.v2.SprintService>();
            services.AddScoped<Services.v2.ProjectService, Services.v2.ProjectService>();
            services.AddScoped<Services.v2.EpicService, Services.v2.EpicService>();
            services.AddScoped<Services.v2.PathService, Services.v2.PathService>();


            // filters
            services.AddScoped<Filters.ProjectUrlBasedAuthorizationFilter>();

            // web socket related
            services.AddScoped<RealtimeRequestFilter, RealtimeRequestFilter>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            // bookmarked this
            // app.UseHttpsRedirection();

            app.UseForwardedHeaders(new ForwardedHeadersOptions {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });


            app.UseRouting();
            app.UseWebSockets();
            app.UseMiddleware<WebsocketHandlerMiddleware>();

            app.UseCors(AllowSpecificOriginCorsPolicy);

            app.UseAuthorization();

            app.UseMiddleware<JwtTokenMiddleware>(); 

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
