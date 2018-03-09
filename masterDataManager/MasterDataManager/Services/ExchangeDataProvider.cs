using DataLayer.Infrastructure.Interfaces;
using DataLayer.Models;
using MasterDataManager.Services.Interfaces;
using MasterDataManager.Services.ServiceModels;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterDataManager.Services
{
    public class ExchangeDataProvider : IExchangeDataProvider
    {
        private IExchangeRepository _exchangeRepository;
        private IMemoryCache _memoryCache;

        public ExchangeDataProvider(
            IExchangeRepository exchangeRepository,
            IMemoryCache memoryCache)
        {
            _exchangeRepository = exchangeRepository;
            _memoryCache = memoryCache;
        }

        private ExchangeDataDict GetExchange(string exchangeName)
        {
            return _memoryCache.GetOrCreate(exchangeName, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromDays(2);
                var exchange = _exchangeRepository.GetByName(exchangeName);
                return new ExchangeDataDict
                {
                    Markets = exchange.ExchangeMarkets?.ToDictionary(o => o.ExchangeMarketCode, o => o.Market),
                    Currencies = exchange.ExchangeCurrencies?.ToDictionary(o => o.ExchangeCurrencyCode, o => o.Currency),
                    Exchange = exchange
                };
            });
        }

        public IEnumerable<Market> GetMarkets(string exchangeName)
        {
            return GetExchange(exchangeName)?.Exchange.ExchangeMarkets.Select(o => o.Market);
        }

        public IEnumerable<ExchangeMarket> GetExchangeMarkets(string exchangeName)
        {
            return GetExchange(exchangeName)?.Exchange.ExchangeMarkets;
        }

        public IEnumerable<Currency> GetCurrencies(string exchangeName)
        {
            return GetExchange(exchangeName)?.Exchange.ExchangeCurrencies.Select(o => o.Currency);
        }

        public IEnumerable<ExchangeCurrency> GetExchangeCurrencies(string exchangeName)
        {
            return GetExchange(exchangeName)?.Exchange.ExchangeCurrencies;
        }

        public Currency GetCurrency(string exchangeName, string currencyCode)
        {
            return GetExchange(exchangeName)?.Currencies[currencyCode];
        }

        public Market GetMarket(string exchangeName, string marketCode)
        {
            return GetExchange(exchangeName)?.Markets[marketCode];
        }
    }
}
