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
using DataLayer;
using Microsoft.EntityFrameworkCore;
using DataLayer.Repositories.Interfaces;
using DataLayer.Repositories;

namespace MarketDataProvider
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
            services.AddDbContext<MarketDataContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("Sqlite")));

            services.AddScoped<IMarketRepository, MarketRepository>();
            services.AddScoped<IExchangeRepository, ExchangeRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IExchangeCurrencyRepository, ExchangeCurrencyRepository>();
            services.AddScoped<IExchangeMarketRepository, ExchangeMarketRepository>();
            //services.AddSingleton<IHostedService, StrategyEvaluationService>();

            services.AddCors();

            services.AddMvc();
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

            app.UseMvc();
        }
    }
}
