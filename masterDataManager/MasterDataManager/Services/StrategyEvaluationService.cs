using DataLayer.Enums;
using DataLayer.Infrastructure.Interfaces;
using DataLayer.Models;
using MasterDataManager.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MasterDataManager.Services
{
    public class StrategyEvaluationService : HostedService
    {
        private HttpClient _client;
        public IServiceProvider Services { get; }

        public StrategyEvaluationService(IServiceProvider services)
        {
            _client = new HttpClient();
            Services = services;

        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var timeStamp = DateTime.Now;

                var response = await _client.GetStringAsync("https://www.binance.com/api/v3/ticker/price");
                //TODO: Exception
                var ticks = JsonConvert.DeserializeObject<List<Tick>>(response);

                var btcDict = ticks.Where(o => o.symbol.EndsWith("BTC"))
                    .ToDictionary(o => o.symbol.Remove(o.symbol.Length - 3), o => Double.Parse(o.price));
                var usdtDict = ticks.Where(o => o.symbol.EndsWith("USDT"))
                    .ToDictionary(o => o.symbol.Remove(o.symbol.Length - 4), o => Double.Parse(o.price));

                var btcPrice = usdtDict["BTC"];

                //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/hosted-services
                using (var scope = Services.CreateScope())
                {
                    var strategyRepository = scope.ServiceProvider
                        .GetRequiredService<IStrategyRepository>();

                    var strategies = strategyRepository.GetAllForEvaluation()
                        .Where(o => o.StrategyState != StrategyState.Stopped);

                    foreach(var strategy in strategies)
                    {
                        double btcSum;
                        try
                        {
                            btcSum = strategy.StrategyAssets.Sum(o => o.Amount * btcDict[o.UserAsset.Currency.Code]);
                        } catch(Exception ex)
                        {
                            btcSum = -1;
                        }
                        var usdSum = btcSum == -1 ? -1 : btcSum * btcPrice;

                        strategy.Evaluation.ToList().Add(new EvaluationTick
                        {
                            TimeStamp = timeStamp,
                            BtcValue = btcSum,
                            UsdValue = usdSum
                        });
                        strategyRepository.EditNotSave(strategy);
                    }
                    strategyRepository.Save();
                }

                await Task.Delay(TimeSpan.FromMinutes(5), cancellationToken);
            }
        }
    }
}
