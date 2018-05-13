using DataLayer.Services.Interfaces;
using MarketDataProvider.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketDataProvider.Enums;
using Microsoft.AspNetCore.WebUtilities;

namespace MarketDataProvider.Services.PriceProviders
{
    public class BinancePriceProvider : PriceProvider
    {
        private IEnumerable<BinanceTick> _ticks;
        private string _exchangeName = "binance";

        private IMarketDataMemCacheService _marketDataMemCacheService;
        public BinancePriceProvider(IMarketDataMemCacheService marketDataMemCacheService) 
           : base() {
            _marketDataMemCacheService = marketDataMemCacheService;
            _ticks = new List<BinanceTick>();
        }

        public override IEnumerable<Tick> GetMarketPrices(string market)
        {
            return _ticks.Where(o => o.symbol.EndsWith(market.ToUpper())).Select(o => new Tick
            {
                Price = decimal.Parse(o.price),
                Market = market,
                Currency = o.symbol.Substring(0, o.symbol.Length - market.Length)
            });
        }

        public override decimal? GetPrice(string symbol)
        {
            var binanceSymbol = _marketDataMemCacheService.GetExchange(_exchangeName)?.Markets
                .FirstOrDefault(o => o.Id.ToUpper() == symbol.ToUpper())?.MarketExchangeId;
            if (binanceSymbol == null) return null;
 
            decimal price;
            if (decimal.TryParse(_ticks.FirstOrDefault(o => o.symbol.ToUpper() == binanceSymbol.ToUpper()).price, out price)){
                return price;
            }
            else return null;
        }

        public override decimal? GetPrice(string market, string currency)
        {
            var symbol = market + "_" + currency;
            return GetPrice(symbol);
        }

        public override async Task UpdatePrices()
        {
            var binancePrices = await _client.GetStringAsync("https://www.binance.com/api/v3/ticker/price");
            _ticks = JsonConvert.DeserializeObject<List<BinanceTick>>(binancePrices);
        }

        public override string GetUrl(OrderType orderType, string market, string currency, decimal amount)
        {
            var url = QueryHelpers.AddQueryString(String.Empty, new Dictionary<string, string>
            {
                { "side", orderType.ToString() },
                { "amount", amount.ToString() },
                //......
            });
            return url;



        }
    }
}
