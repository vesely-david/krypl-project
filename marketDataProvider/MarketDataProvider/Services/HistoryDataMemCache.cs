using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace MarketDataProvider.Services
{
    public class HistoryDataMemCache : IHistoryDataMemCache
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private IMemoryCache _memoryCache;

        public HistoryDataMemCache(
            IServiceScopeFactory scopeFactory,
            IMemoryCache memoryCache)
        {
            _scopeFactory = scopeFactory;
            _memoryCache = memoryCache;
        }

        public IEnumerable<ExchangeMemCache> ExchangeList()
        {
            var ex = ExchangeIds();
            return ex.Select(GetExchange);
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

                    var currencies = exchange.ExchangeCurrencies.Select(o => new CurrencyMemCache
                    {
                        CurrencyExchangeId = o.CurrencyExchangeId,
                        Id = o.CurrencyId,
                        Name = o.Currency.Name
                    }).ToList();

                    var markets = exchange.ExchangeMarkets.Select(o => new MarketMemCache
                    {
                        MarketExchangeId = o.MarketExchangeId,
                        Id = o.MarketId,
                        CurrencyId = o.Market.CurrencyId,
                        MarketCurrencyId = o.Market.MarketCurrencyId,
                    }).ToList();

                    return new ExchangeMemCache(markets, currencies)
                    {
                        Id = exchange.Id,
                        Name = exchange.Name,
                        ProvidesFullHistoryData = exchange.ProvidesFullHistoryData,
                        Web = exchange.Web,
                    };
                }
            });
        }
    }
}
