using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DataLayer.Enums;
using DataLayer.Infrastructure.Interfaces;
using DataLayer.Models;
using MasterDataManager.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MasterDataManager.Services
{
    public class PaperTradeCheckerService : HostedService
    {
        private HttpClient _client;
        public IServiceProvider Services { get; }
        private readonly IServiceScopeFactory ScopeFactory;

        public PaperTradeCheckerService(IServiceProvider services, IServiceScopeFactory scopeFactory)
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

                    var marketDataService = scope.ServiceProvider.GetRequiredService<IMarketDataService>();
                    var currentRates = await marketDataService.GetCurrentRates("binance");

                    var tradeExecutor = scope.ServiceProvider.GetRequiredService<ITradeFinalizationService>();

                    foreach(var trade in openedTrades)
                    {
                        if (!currentRates.ContainsKey(trade.MarketId)) continue;
                        if((trade.OrderType == OrderType.Buy && currentRates[trade.MarketId] <= trade.Price) ||
                            (trade.OrderType == OrderType.Sell && currentRates[trade.MarketId] >= trade.Price))
                        {
                            tradeExecutor.ExecuteTrade(trade, currentRates[trade.MarketId]);
                        }
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
            }
        }
    }
}
