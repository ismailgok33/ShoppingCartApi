
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using CicekSepetiTask.Autofac;
using CicekSepetiTask.Repositories;
using CicekSepetiTask.Services;
using CicekSepetiTask.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CicekSepetiTask
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
            services.AddCors();

            var appSettingSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingSection);
            var appSettings = appSettingSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // var connection = @"Data Source=demoapi-db;Initial Catalog=CicekSepetiDB;Integrated Security=False;User ID=sa;Password=MssqlPass123!";
            var connection = @"Server=localhost:1433;Database=CicekSepetiDB;User=sa;Password=MssqlPass123!;";
            var connection2 = @"Data Source=host.docker.internal,1433;Initial Catalog = CicekSepetiDB;Persist Security Info=False;User Id=sa;Password=MssqlPass123!;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30";
            //var server = Configuration["DatabaseServer"];
            //var database = Configuration["DatabaseName"];
            //var user = Configuration["DatabaseUser"];
            //var password = Configuration["DatabaseUserPassword"];
            //var connection = String.Format("Server={0};Database={1};User={2};Password={3};", server, database, user, password);
            services.AddDbContext<DataContext>(x => x.UseSqlServer(connection2));
            // services.AddDbContext<DataContext>(x => x.UseSqlServer(Configuration.GetConnectionString("ConnectionString")));
            // services.AddDbContext<DataContext>(x => x.UseSqlServer(Configuration["ConnectionString"]));

            // var container = new ContainerBuilder();
            // container.Populate(services);

            // var connectionString = Configuration["ConnectionString"];
            // container.RegisterModule(new ApplicationModule(connectionString));
            // container.Register(p => Configuration.Get<AppSettings>()).SingleInstance();

            // // services.AddDbContext<DataContext>();
            services.AddControllers();
            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<IUserService, UserService>();

            //services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddStackExchangeRedisCache(action =>
            {
                action.InstanceName = "Redis";
                action.Configuration = "127.0.0.1:6379";
            });
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen();
            // services.AddSwaggerGen(options =>
            // {
            //     options.SwaggerDoc(appSettings.ApiVersion, new OpenApiInfo { Title = appSettings.ApiName, Version = appSettings.ApiVersion });

            //     options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            //     {
            //         Type = SecuritySchemeType.OAuth2,
            //         Flows = new OpenApiOAuthFlows
            //         {
            //             AuthorizationCode = new OpenApiOAuthFlow
            //             {
            //                 AuthorizationUrl = new Uri($"{appSettings.AuthBaseUrl}/users/authorize"),
            //                 TokenUrl = new Uri($"{appSettings.AuthBaseUrl}/users/token"),
            //                 Scopes = new Dictionary<string, string> {
            //                     { appSettings.OidcApiName, appSettings.ApiName }
            //                 }
            //             }
            //         }
            //     });
            //     options.OperationFilter<AuthorizeCheckOperationFilter>();
            // });

            // container.Build();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            //app.UseHttpsRedirection();

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
