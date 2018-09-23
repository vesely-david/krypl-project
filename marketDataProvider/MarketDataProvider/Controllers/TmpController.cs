using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataLayer.Repositories.Interfaces;
using DataLayer.Models;
using Newtonsoft.Json;
using MarketDataProvider.Services.Models;
using System.Net.Http;

namespace MarketDataProvider.Controllers
{
    [Produces("application/json")]
    [Route("admin")]
    public class AdminController : Controller
    {
        private IExchangeRepository _repo;
        private HttpClient _httpClient;

        public AdminController(IExchangeRepository repo)
        {
            _repo = repo;
            _httpClient = new HttpClient();
        }

        [HttpPost]
        [Route("seedBinance")]
        public async Task<IActionResult> SeedBinance()
        {
            var binancePrices = _httpClient.GetStringAsync("https://www.binance.com/api/v3/ticker/price");
            var coinMarketCapData = _httpClient.GetStringAsync("https://api.coinmarketcap.com/v2/listings/");

            await Task.WhenAll(binancePrices, coinMarketCapData);

            var cmcTemplate = new { data = new[] { new { name= "", symbol=""} }};
            var cmcData = JsonConvert.DeserializeAnonymousType(coinMarketCapData.Result, cmcTemplate).data;
            var markets = new string[] { "BTC", "ETH", "BNB", "USDT" };
            var ticks = JsonConvert.DeserializeObject<List<BinanceTick>>(binancePrices.Result);

            var binance = new Exchange
            {
                Id = "binance",
                Web = "www.binance.com",
                Name = "Binance",
                ProvidesFullHistoryData = true,
                ExchangeCurrencies = new List<ExchangeCurrency>(),
                ExchangeMarkets = new List<ExchangeMarket>()
            };

            var notResolved = new List<string>();
            var moreOccurrences = new List<string>();
            var resolved = new List<KeyValuePair<string, string>>();

            foreach (var market in markets)
            {
                var length = market.Length;
                var currencies = ticks.Where(o => o.symbol.EndsWith(market))
                    .Select(o => o.symbol.Substring(0, o.symbol.Length - length));
                foreach(var c in currencies)
                {
                    var matches = cmcData.Where(o => o.symbol == c);
                    resolved.Add(new KeyValuePair<string, string>(c, matches.Count() == 1 ? matches.First().name : ""));
                    if (matches.Count() == 0) notResolved.Add(c);
                    if (matches.Count() > 1) moreOccurrences.Add(c);
                }
            }

            binance.ExchangeCurrencies = resolved.Distinct().Select(o => new ExchangeCurrency
            {
                Currency = new Currency
                {
                    Id = o.Key.ToUpper(),
                    Name = o.Value,
                },
                Id = "binance_" + o.Key.ToUpper(),
                CurrencyExchangeId = o.Key,
            }).ToList();

            binance.ExchangeCurrencies.Add(new ExchangeCurrency //Base Currency
            {
                Currency = new Currency
                {
                    Id = "USDT",
                    Name = "Tether"
                },
                Id = "binance_USDT",
                CurrencyExchangeId = "USDT",
            });

            foreach (var market in markets)
            {
                var length = market.Length;
                var currencies = ticks.Where(o => o.symbol.EndsWith(market))
                    .Select(o => o.symbol.Substring(0, o.symbol.Length - length));
                foreach (var c in currencies)
                {
                    var matches = cmcData.Where(o => o.symbol == c);
                    binance.ExchangeMarkets.Add(new ExchangeMarket
                    {
                        Id = "binance_" + market.ToUpper() + "_" + c.ToUpper(),
                        MarketExchangeId = c + market,
                        Market = new Market
                        {
                            Id = market.ToUpper() + "_" + c.ToUpper(),
                            CurrencyId = c.ToUpper(),
                            MarketCurrencyId = market.ToUpper()
                        }
                    });
                }
            }
            _repo.Add(binance);
            return Ok(new
            {
                notResolved = notResolved.Distinct(),   
                moreOccurrences = moreOccurrences.Distinct(),
            });
        }


        [HttpPost]
        [Route("seed")]
        public IActionResult Seed()
        {
            var btc = new Currency
            {
                Id = "btc",
                Name = "Bitcoin"
            };
            var eth = new Currency
            {
                Id = "eth",
                Name = "Ethereum"
            };
            var ltc = new Currency
            {
                Id = "ltc",
                Name = "Litecoin"
            };
            var trx = new Currency
            {
                Id = "trx",
                Name = "Tron"
            };


            var binance = new Exchange
            {
                Id = "binance",
                Web = "www.binance.com",
                Name = "Binance",
                ProvidesFullHistoryData = true,
                ExchangeCurrencies = new List<ExchangeCurrency>
                {
                    new ExchangeCurrency
                    {
                        Currency = btc,
                        CurrencyExchangeId = "BTC",
                        Id="binance_btc",
                    },
                    new ExchangeCurrency
                    {
                        Currency = eth,
                        CurrencyExchangeId = "ETH",
                        Id="binance_eth",
                    },
                    new ExchangeCurrency
                    {
                        Currency = ltc,
                        CurrencyExchangeId = "LTC",
                        Id="binance_ltc",
                    },
                    new ExchangeCurrency
                    {
                        Currency = trx,
                        CurrencyExchangeId = "TRX",
                        Id="binance_trx",
                    },
                },
                ExchangeMarkets = new List<ExchangeMarket>
                {
                    new ExchangeMarket
                    {
                        Market = new Market
                        {
                            MarketCurrency = btc,
                            Currency = eth,
                            Id = "btc_eth"
                        },
                        Id = "binance_btc_eth",
                        MarketExchangeId = "ETHBTC",
                    },
                    new ExchangeMarket
                    {
                        Market = new Market
                        {
                            MarketCurrency = btc,
                            Currency = ltc,
                            Id = "btc_ltc"
                        },
                        Id = "binance_btc_ltc",
                        MarketExchangeId = "LTCBTC",
                    },
                    new ExchangeMarket
                    {
                        Market = new Market
                        {
                            MarketCurrency = btc,
                            Currency = trx,
                            Id = "btc_trx"
                        },
                        Id = "binance_btc_trx",
                        MarketExchangeId = "TRXBTC",
                    },
                    new ExchangeMarket
                    {
                        Market = new Market
                        {
                            MarketCurrency = eth,
                            Currency = ltc,
                            Id = "eth_ltc"
                        },
                        Id = "binance_eth_ltc",
                        MarketExchangeId = "LTCETH",
                    },
                },
            };
            _repo.Add(binance);

            return Ok();
        }
    }
}