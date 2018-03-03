using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using DataLayer.Infrastructure.Interfaces;
using DataLayer.Infrastructure;
using MasterDataManager.Services;
using MasterDataManager.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DataLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Web.Http.Cors;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Caching.Memory;

namespace MasterDataManager
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
            services.AddDbContext<MasterDataContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("Sqlite")));

            services.AddScoped<IStrategyRepository, StrategyRepository>();
            services.AddScoped<ITradeRepository, TradeRepository>();
            services.AddScoped<IUserAssetRepository, UserAssetRepository>();
            services.AddScoped<IMarketRepository, MarketRepository>();
            services.AddScoped<IExchangeRepository, ExchangeRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddSingleton<IHostedService, StrategyEvaluationService>();
            services.AddScoped<IExchangeDataProvider, ExchangeDataProvider>();
            services.AddScoped<IExchangeObjectFactory, ExchangeObjectFactory>();
            services.AddScoped<BinanceService>();
            services.AddScoped<BittrexService>();

            services.AddSingleton(Configuration);

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<MasterDataContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            }).AddJwtBearer("JwtBearer", jwtBearerOptions =>
            {
                var securityKey = Configuration["JwtTokens:Secret"];
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey)),
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true, //validate the expiration and not before values in the token
                    ClockSkew = TimeSpan.FromMinutes(5) //5 minute tolerance for the expiration date
                };
            });

            services.AddCors();

            services.AddMemoryCache();

            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Trading API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(builder =>
                builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .SetPreflightMaxAge(TimeSpan.FromHours(1)));
            app.UseAuthentication(); //needs to be up in the pipeline, before MVC
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Trading API V1");
            });

            app.UseMvc();
        }
    }
}
