using System;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using Edorator.Services.Contracts;
using Edorator.Services.Data;
using Edorator.Services.Handlers;
using Edorator.Services.Handlers.Base;
using Edorator.Services.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Edorator.Services
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public Startup(IConfigurationRoot configuration)
        {
            this.Configuration = configuration;
        }
        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            });

            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
            });

            services.AddCors(builder => builder.AddPolicy("EdoratorUI", policy => {
                policy.WithOrigins("http://localhost:3000");
                policy.AllowAnyMethod();
                policy.AllowAnyHeader();
                policy.AllowCredentials();
            }));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);
            services.AddSingleton<IHandler<AddServiceRequest, AddServiceResponse>, AddServiceHandler>();
            services.AddSingleton<IHandler<GetServicesRequest, GetServicesResponse>, GetServicesHandler>();


            services.Configure<Settings>(options =>
            {
                options.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                options.Database = Configuration.GetSection("MongoConnection:Database").Value;
            });

            services.AddTransient<IServiceRepository, ServiceRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseJwtBearerAuthentication(new JwtBearerOptions()
            {
                TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = TokenAuthOption.Key,
                    ValidAudience = TokenAuthOption.Audience,
                    ValidIssuer = TokenAuthOption.Issuer,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(0)
                }
            });

            app.UseMvc();

             app.UseCors("EdoratorUI");

            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            app.UseSwagger();
            app.UseSwaggerUi();
        }
    }

    public class TokenAuthOption
    {
        public static string Audience { get; } = "ExampleAudience";
        public static string Issuer { get; } = "ExampleIssuer";
        public static SymmetricSecurityKey Key { get; } = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("testtesttesttesttesttesttesttest"));
    }

    public class RSAKeyHelper
    {
        public static RSAParameters GenerateKey()
        {
            using (var key = new RSACryptoServiceProvider(2048))
            {
                return key.ExportParameters(true);
            }
        }
    }
}
