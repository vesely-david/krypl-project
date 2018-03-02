using DataLayer.Infrastructure.Interfaces;
using DataLayer.Models;
using MasterDataManager.Services.Interfaces;
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

        public Exchange GetExchange(string exchangeName)
        {
            return _memoryCache.GetOrCreate(exchangeName, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromDays(2);
                return _exchangeRepository.GetByName(exchangeName);
            });
        }

        public IEnumerable<Market> GetMarkets(string exchangeName)
        {
            return GetExchange(exchangeName)?.ExchangeMarkets.Select(o => o.Market);
        }

        public IEnumerable<ExchangeMarket> GetExchangeMarkets(string exchangeName)
        {
            return GetExchange(exchangeName)?.ExchangeMarkets;
        }
    }
}
