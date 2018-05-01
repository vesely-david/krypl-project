using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataLayer.Repositories.Interfaces;
using DataLayer.Models;

namespace MarketDataProvider.Controllers
{
    [Produces("application/json")]
    [Route("api/tmp")]
    public class TmpController : Controller
    {
        private IExchangeRepository _repo;
        public TmpController(IExchangeRepository repo)
        {
            _repo = repo;
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