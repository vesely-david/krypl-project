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
        private IMarketRepository _marketRepository;
        private ICurrencyRepository _currencyRepository;
        private IExchangeCurrencyRepository _exchangeCurrencyRepository;
        private IExchangeMarketRepository _exchangeMarketRepository;

        public AdminController(IExchangeRepository repo, 
            IMarketRepository marketRepository, 
            ICurrencyRepository currencyRepository,
            IExchangeCurrencyRepository exchangeCurrencyRepository,
            IExchangeMarketRepository exchangeMarketRepository)
        {
            _repo = repo;
            _marketRepository = marketRepository;
            _currencyRepository = currencyRepository;
            _exchangeCurrencyRepository = exchangeCurrencyRepository;
            _exchangeMarketRepository = exchangeMarketRepository;
            _httpClient = new HttpClient();
        }

        [HttpPost]
        [Route("seedBinance")]
        public async Task<IActionResult> SeedBinance()
        {
            var coinMarketCapData = _httpClient.GetStringAsync("https://api.coinmarketcap.com/v2/listings/");
            var binanceInfo = _httpClient.GetStringAsync("https://www.binance.com/api/v1/exchangeInfo");

            await Task.WhenAll(binanceInfo, coinMarketCapData);

            var cmcTemplate = new { data = new[] { new { name= "", symbol=""} }};
            var cmcData = JsonConvert.DeserializeAnonymousType(coinMarketCapData.Result, cmcTemplate).data;
            var binanceInfoTemplate = new { symbols = new[] { new { symbol = "", baseAsset = "", quoteAsset = "" } } };

            var symbols = JsonConvert.DeserializeAnonymousType(binanceInfo.Result, binanceInfoTemplate).symbols;

            var binance = new Exchange
            {
                Id = "binance",
                Web = "www.binance.com",
                Name = "Binance",
                ProvidesFullHistoryData = true,
            };

            _repo.Add(binance);

            var errors = new List<string>();

            var currencies = symbols.Aggregate(new HashSet<string>(), (res, val) => //Get unique currencies
            {
                res.Add(val.baseAsset);
                res.Add(val.quoteAsset);
                return res;
            });

            foreach(var c in currencies) // Set currencies
            {

                var cmcMatches = cmcData.Where(o => o.symbol == c);
                if (cmcMatches.Count() != 1) errors.Add(c);


                _currencyRepository.AddNotSave(new Currency
                {
                    Id = c,
                    Name = cmcMatches.FirstOrDefault()?.name ?? c
                });
            }
            _currencyRepository.Save();

            foreach(var c in currencies) // Set exchangeCurrencies
            {
                _exchangeCurrencyRepository.AddNotSave(new ExchangeCurrency
                {
                    Id = "binance_" + c,
                    CurrencyId = c,
                    ExchangeId = "binance",
                    CurrencyExchangeId = c
                });
            }
            _exchangeCurrencyRepository.Save();

            foreach (var m in symbols) // Set markets
            {
                _marketRepository.AddNotSave(new Market
                {
                    MarketCurrencyId = m.quoteAsset,
                    CurrencyId = m.baseAsset,
                    Id = m.quoteAsset + "_" + m.baseAsset,
                });
            }
            _marketRepository.Save();

            foreach(var m in symbols) // Set exchange markets 
            {
                _exchangeMarketRepository.AddNotSave(new ExchangeMarket
                {
                    ExchangeId = "binance",
                    Id = "binance_" + m.quoteAsset + "_" + m.baseAsset,
                    MarketExchangeId = m.symbol,
                    MarketId = m.quoteAsset + "_" + m.baseAsset
                });
            }
            _exchangeMarketRepository.Save();

            return Ok(errors);
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