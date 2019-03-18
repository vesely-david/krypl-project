﻿using DataLayer;
using DataLayer.Enums;
using DataLayer.Infrastructure;
using DataLayer.Infrastructure.Interfaces;
using DataLayer.Models;
using MasterDataManager.Models;
using MasterDataManager.Services.Interfaces;
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
        private readonly IServiceScopeFactory ScopeFactory;

        public StrategyEvaluationService(IServiceProvider services, IServiceScopeFactory scopeFactory)
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
                    var marketDataService = scope.ServiceProvider
                        .GetRequiredService<IMarketDataService>();
                    var currentPrices = await marketDataService.GetCurrentPrices("binance");

                    var strategyRepository = scope.ServiceProvider
                        .GetRequiredService<IStrategyRepository>();

                    var strategies = strategyRepository.GetAllForEvaluation()
                        .Where(o => o.StrategyState != StrategyState.Stopped);
                    var timeStamp = DateTime.Now;

                    var valueSums = new Dictionary<TradingMode, (decimal btcValue, decimal usdValue)>
                    {
                        {TradingMode.Real, (btcValue: 0, usdValue: 0)},
                        {TradingMode.PaperTesting, (btcValue: 0, usdValue: 0)},
                        {TradingMode.BackTesting, (btcValue: 0, usdValue: 0)},
                    };

                    foreach (var strategy in strategies.Where(o => !o.IsOverview)) // Strategies
                    {
                        var valueSum = strategy.StrategyAssets.Aggregate((btcSum: 0m, usdSum: 0m), (res, val) =>
                        {
                            if (currentPrices.ContainsKey(val.UserAsset.Currency))
                            {
                                res.btcSum += val.Amount * currentPrices[val.UserAsset.Currency].BtcValue;
                                res.usdSum += val.Amount * currentPrices[val.UserAsset.Currency].UsdValue;
                            }
                            return res;
                        });
                        strategy.Evaluations.Add(new EvaluationTick
                        {
                            TimeStamp = timeStamp,
                            BtcValue = valueSum.btcSum,
                            UsdValue = valueSum.usdSum
                        });
                        strategyRepository.EditNotSave(strategy);
                    }
                    foreach (var strategy in strategies.Where(o => o.IsOverview)) // Overviews
                    {
                        var (btcSum, usdSum) = strategy.User.UserAssets.Where(o => o.TradingMode == strategy.TradingMode)
                            .Aggregate((btcSum: 0m, usdSum: 0m), (res, val) =>
                        {
                            if (currentPrices.ContainsKey(val.Currency))
                            {
                                res.btcSum += val.Amount * currentPrices[val.Currency].BtcValue;
                                res.usdSum += val.Amount * currentPrices[val.Currency].UsdValue;
                            }
                            return res;
                        });
                        strategy.Evaluations.Add(new EvaluationTick
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
