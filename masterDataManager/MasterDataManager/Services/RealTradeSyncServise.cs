using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DataLayer.Enums;
using DataLayer.Infrastructure.Interfaces;
using DataLayer.Models;
using MasterDataManager.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MasterDataManager.Services
{
    public class RealTradeSyncServise : HostedService
    {
        private HttpClient _client;
        public IServiceProvider Services { get; }
        private readonly IServiceScopeFactory ScopeFactory;

        public RealTradeSyncServise(IServiceProvider services, IServiceScopeFactory scopeFactory)
        {
            _client = new HttpClient();
            Services = services;
            ScopeFactory = scopeFactory;
        }


        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                //https://www.stevejgordon.co.uk/asp-net-core-2-ihostedservice
                //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/hosted-services
                using (var scope = ScopeFactory.CreateScope())
                {
                    var tradeRepo = scope.ServiceProvider.GetRequiredService<ITradeRepository>();
                    var openedTrades = tradeRepo.GetOpenedPaperTrades();
                    var openedPoloniex = openedTrades.Where(o => o.Exchange == "poloniex").Select(o => o.Strategy.UserId).Distinct();
                    var openedBinance = openedTrades.Where(o => o.Exchange == "binance").Select(o => o.Strategy.UserId).Distinct();

                    var exchangeObjectFactory = scope.ServiceProvider.GetRequiredService<IExchangeObjectFactory>();
                    var binance = exchangeObjectFactory.GetExchange("binance");
                    var poloniex = exchangeObjectFactory.GetExchange("poloniex");

                    await Task.WhenAll(openedPoloniex.Select(o => poloniex.MirrorTrades(o)));
                    await Task.WhenAll(openedBinance.Select(o => binance.MirrorTrades(o)));
   
                }

                await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
            }
        }
    }
}
