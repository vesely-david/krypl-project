using DataLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using DataLayer.Models;
using DataLayer.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using DataLayer.Services.Models;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace DataLayer.Services
{
    public class MarketDataMemCacheService : IMarketDataMemCacheService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private IMemoryCache _memoryCache;

        public MarketDataMemCacheService(
            IServiceScopeFactory scopeFactory,
            IMemoryCache memoryCache
            )
        {
            _scopeFactory = scopeFactory;
            _memoryCache = memoryCache;
        }

        public IEnumerable<ExchangeMemCache> ExchangeList()
        {
            var ex = ExchangeIds();
            return ex.Select(o => GetExchange(o));
        }
        private IEnumerable<string> ExchangeIds()
        {
            return _memoryCache.GetOrCreate("#exchanges#", entry =>
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var exchangeRepository = scope.ServiceProvider.GetRequiredService<IExchangeRepository>();
                    entry.SlidingExpiration = TimeSpan.FromDays(1);
                    return new List<string>(exchangeRepository.List().Select(o => o.Id));
                }
            });
        }

        public ExchangeMemCache GetExchange(string exchangeId)
        {
            return _memoryCache.GetOrCreate(exchangeId, entry =>
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var exchangeRepository = scope.ServiceProvider.GetRequiredService<IExchangeRepository>();
                    entry.SlidingExpiration = TimeSpan.FromDays(1);
                    var exchange = exchangeRepository.GetForMemCache(exchangeId);
                    return exchange == null ? null : new ExchangeMemCache
                    {
                        Id = exchange.Id,
                        Name = exchange.Name,
                        ProvidesFullHistoryData = exchange.ProvidesFullHistoryData,
                        Web = exchange.Web,
                        Currencies = exchange.ExchangeCurrencies.Select(o => new CurrencyMemCache
                        {
                            CurrencyExchangeId = o.CurrencyExchangeId,
                            Id = o.CurrencyId,
                            Name = o.Currency.Name
                        }).ToList(),
                        Markets = exchange.ExchangeMarkets.Select(o => new MarketMemCache
                        {
                            MarketExchangeId = o.MarketExchangeId,
                            Id = o.MarketId,
                            CurrencyId = o.Market.CurrencyId,
                            MarketCurrencyId = o.Market.MarketCurrencyId,
                        }).ToList()
                    };
                }
            });
        }
    }
}
