using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DataLayer.Models;
using DataLayer.Infrastructure.Interfaces;
using MasterDataManager.Models;
using Microsoft.AspNetCore.Identity;
using DataLayer.Enums;

namespace MasterDataManager.Controllers
{
    [Produces("application/json")]
    [Route("api/admin")]
    [Authorize]
    public class AdminController : Controller
    {
        private UserManager<User> _userManager;
        private IExchangeRepository _exchangeRepository;
        private ICurrencyRepository _currencyRepository;
        private IMarketRepository _marketRepository;
        private IUserAssetRepository _userAssetRepository;
        private IExchangeSecretRepository _exchangeSecretRepository;
        private IExchangeCurrencyRepository _exchangeCurrencyRepository;
        private IExchangeMarketRepository _exchangeMarketRepository;

        public AdminController(
            UserManager<User> userManager,
            IExchangeRepository exchangeRepository,
            IMarketRepository marketRepository,
            ICurrencyRepository currencyRepository,
            IUserAssetRepository userAssetRepository,
            IExchangeSecretRepository exchangeSecretRepository,
            IExchangeCurrencyRepository exchangeCurrencyRepository,
            IExchangeMarketRepository exchangeMarketRepository)
        {
            _userManager = userManager;
            _exchangeRepository = exchangeRepository;
            _currencyRepository = currencyRepository;
            _marketRepository = marketRepository;
            _userAssetRepository = userAssetRepository;
            _exchangeSecretRepository = exchangeSecretRepository;
            _exchangeCurrencyRepository = exchangeCurrencyRepository;
            _exchangeMarketRepository = exchangeMarketRepository;
        }

        [HttpGet("exchange")]
        public IActionResult GetExchanges()
        {
            return Ok(_exchangeRepository.List());
        }

        [HttpPost("exchange")]
        public IActionResult AddExchange([FromBody] Exchange exchange)
        {
            if (exchange.Name == String.Empty || exchange.Web == String.Empty)
                return BadRequest("Missing info");
            if (_exchangeRepository.GetByName(exchange.Name) != null)
                return BadRequest("Exchange already exists");

            _exchangeRepository.Add(exchange);
            return Ok(exchange);
        }

        [HttpGet("currency")]
        public JsonResult GetCurrencies()
        {
            return Json(_currencyRepository.List());
        }
        /*

        [HttpPost("currency")]
        public JsonResult AddCurrency([FromBody] Currency currency)
        {
            if (currency.Name == String.Empty || currency.Code == String.Empty)
                return Json(new JsonResponse { success = false, message = "Missing info" });
            if (_currencyRepository.GetByCode(currency.Code) != null)
                return Json(new JsonResponse { success = false, message = "Code already exists" });

            _currencyRepository.Add(currency);
            return Json(new JsonResponse { success = true, response = currency });
        }

        [HttpGet("market")]
        public JsonResult GetMarkets()
        {
            return Json(_marketRepository.List());
        }

        [HttpPost("currency")]
        public JsonResult AddMarket(string baseCurrencyCode, string secondaryCurrencyCode)
        {
            if (baseCurrencyCode == String.Empty || secondaryCurrencyCode == String.Empty)
                return Json(new JsonResponse { success = false, message = "Missing info" });
            var baseCurrency = _currencyRepository.GetByCode(baseCurrencyCode);
            var secondaryCurrency = _currencyRepository.GetByCode(secondaryCurrencyCode);
            if (baseCurrency == null || secondaryCurrency == null)
                return Json(new JsonResponse { success = false, message = "Currency ot found" });
            if(_marketRepository.GetMarketByCurrencies(baseCurrency.Id, secondaryCurrency.Id) != null)
                return Json(new JsonResponse { success = false, message = "Market already exists" });
            var market = new Market
            {
                BaseCurrency = baseCurrency,
                SecondaryCurrency = secondaryCurrency
            };
            _marketRepository.Add(market);
            return Json(new JsonResponse { success = true, response = market});
        }

    */

        //================ SEED METHODS =====================




        [AllowAnonymous]
        [HttpPost("seedEverything")]
        public async Task<StatusCodeResult> SeedEverythign()
        {
            var currencies = SeedCurrencies();
            var exchanges = SeedExchanges();
            var markets = SeedMarkets();
            var users = SeedUsersAsync();
            var exchangeSecrets = SeedExchangeSecrets("kirchjan");
            var marketAndCurrencyConnections = SeedMarketAndCurrencyConections("kirchjan");
            var userAssets = SeedUserAssetsAsync("veselda");

            await users;
            await userAssets;
            await marketAndCurrencyConnections;

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("seedAssets")]
        public async Task<ActionResult> SeedUserAssetsAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound();

            var bittrex = _exchangeRepository.GetByName("binance");

            var btc = _currencyRepository.GetByCode("btc");
            var ltc = _currencyRepository.GetByCode("ltc");
            var pivx = _currencyRepository.GetByCode("pivx");
            var vtc = _currencyRepository.GetByCode("vtc");
            var usdt = _currencyRepository.GetByCode("usdt");

            _userAssetRepository.Add(new UserAsset
            {
                Amount = 1,
                Currency = btc,
                CurrencyId = btc.Id,
                Exchange = bittrex,
                ExchangeId = bittrex.Id,
                User = user,
                UserId = user.Id,
                TradingMode = TradingMode.PaperTesting
            });
            _userAssetRepository.Add(new UserAsset
            {
                Amount = 10,
                Currency = ltc,
                CurrencyId = ltc.Id,
                Exchange = bittrex,
                ExchangeId = bittrex.Id,
                User = user,
                UserId = user.Id,
                TradingMode = TradingMode.PaperTesting

            });
            _userAssetRepository.Add(new UserAsset
            {
                Amount = 75,
                Currency = pivx,
                CurrencyId = pivx.Id,
                Exchange = bittrex,
                ExchangeId = bittrex.Id,
                User = user,
                UserId = user.Id,
                TradingMode = TradingMode.PaperTesting

            });
            _userAssetRepository.Add(new UserAsset
            {
                Amount = 110,
                Currency = vtc,
                CurrencyId = vtc.Id,
                Exchange = bittrex,
                ExchangeId = bittrex.Id,
                User = user,
                UserId = user.Id,
                TradingMode = TradingMode.PaperTesting

            });
            _userAssetRepository.Add(new UserAsset
            {
                Amount = 921,
                Currency = usdt,
                CurrencyId = usdt.Id,
                Exchange = bittrex,
                ExchangeId = bittrex.Id,
                User = user,
                UserId = user.Id,
                TradingMode = TradingMode.PaperTesting

            });


            return Ok();
        }

        [AllowAnonymous]
        [Route("seedUsers")]
        [HttpPost]
        public async Task<StatusCodeResult> SeedUsersAsync()
        {
            var user1 = await _userManager.FindByNameAsync("veselda7");
            if (user1 == null)
            {
                var david = new User
                {
                    Email = "veselda7@gmail.com",
                    EmailConfirmed = true,
                    UserName = "veselda7"
                };
                var a = await _userManager.CreateAsync(david, "veselda7!Hesl0");
            }
            var user2 = await _userManager.FindByNameAsync("kirchjan");
            if (user2 == null)
            {
                var honza = new User
                {
                    Email = "h.kirchner@seznam.cz",
                    EmailConfirmed = true,
                    UserName = "kirchjan",
                };
                var a = await _userManager.CreateAsync(honza, "kirchjan!Hesl0");
            }

            return Ok();
        }

        [HttpPost("seedUserSecrets")]
        public async Task<StatusCodeResult> SeedExchangeSecrets(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            var binance = _exchangeRepository.GetByName("binance");

            //Don't bother to try... It's from Binance API documentation ;)
            _exchangeSecretRepository.Add(new ExchangeSecret
            {
                ApiKey = "vmPUZE6mv9SD5VNHk4HlWFsOr6aKE2zvsw0MuIgwCIPy6utIco14y7Ju91duEh8A",
                ApiSecret = "NhqPtmdSJYdKjVHjA7PZj4Mge3R5YNiP1e3UZjInClVN65XAbvqqM6A7H5fATj0j",
                ExchangeId = binance.Id,
                UserId = user.Id
            });


            return Ok();
        }

        [HttpPost("seedCurrencies")]
        public StatusCodeResult SeedCurrencies()
        {
            _currencyRepository.Add(new Currency
            {
                Name = "Bitcoin",
                Code = "BTC"
            });
            _currencyRepository.Add(new Currency
            {
                Name = "Pivx",
                Code = "PIVX"
            });
            _currencyRepository.Add(new Currency
            {
                Name = "Tron",
                Code = "TRX"
            });
            _currencyRepository.Add(new Currency
            {
                Name = "Vertcoin",
                Code = "VTC"
            });
            _currencyRepository.Add(new Currency
            {
                Name = "Litecoin",
                Code = "LTC"
            });
            _currencyRepository.Add(new Currency
            {
                Name = "Tether",
                Code = "USDT"
            });

            return Ok();
        }

        [HttpPost("seedExchanges")]
        public StatusCodeResult SeedExchanges()
        {
            _exchangeRepository.Add(new Exchange
            {
                Name = "Bittrex",
                Web = "bittrex.com"

            });
            _exchangeRepository.Add(new Exchange
            {
                Name = "Binance",
                Web = "binance.com"
            });
            return Ok();
        }

        [HttpPost("seedMarkets")]
        public StatusCodeResult SeedMarkets()
        {
            var btc = _currencyRepository.GetByCode("BTC");
            var ltc = _currencyRepository.GetByCode("LTC");
            var pivx = _currencyRepository.GetByCode("PIVX");
            var trx = _currencyRepository.GetByCode("TRX");
            var vtc = _currencyRepository.GetByCode("VTC");
            var usdt = _currencyRepository.GetByCode("USDT");

            var btcLtc = new Market
            {
                Code = "BTC_LTC",
                BaseCurrency = btc,
                MarketCurrency = ltc
            };
            var btcPivx = new Market
            {
                Code = "BTC_PIVX",
                BaseCurrency = btc,
                MarketCurrency = pivx
            };
            var btcVtc = new Market
            {
                Code = "BTC_VTC",
                BaseCurrency = btc,
                MarketCurrency = vtc
            };
            var btcTrx = new Market
            {
                Code = "BTC_TRX",
                BaseCurrency = btc,
                MarketCurrency = trx
            };
            var usdtBtc = new Market
            {
                Code = "USDT_BTC",
                BaseCurrency = usdt,
                MarketCurrency = btc
            };
            var usdtLtc = new Market
            {
                Code = "USDT_LTC",
                BaseCurrency = usdt,
                MarketCurrency = ltc,
            };

            _marketRepository.Add(btcLtc);
            _marketRepository.Add(btcPivx);
            _marketRepository.Add(btcTrx);
            _marketRepository.Add(btcVtc);
            _marketRepository.Add(usdtBtc);
            _marketRepository.Add(usdtLtc);

            return Ok();
        }

        public async Task<IActionResult> SeedMarketAndCurrencyConections(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            var bittrex = _exchangeRepository.GetByName("bittrex");
            var binance = _exchangeRepository.GetByName("binance");

            var btc = _currencyRepository.GetByCode("BTC");
            var ltc = _currencyRepository.GetByCode("LTC");
            var pivx = _currencyRepository.GetByCode("PIVX");
            var trx = _currencyRepository.GetByCode("TRX");
            var vtc = _currencyRepository.GetByCode("VTC");
            var usdt = _currencyRepository.GetByCode("USDT");

            var btcLtc = _marketRepository.GetByCode("BTC_LTC");
            var btcPivx = _marketRepository.GetByCode("BTC_PIVX");
            var btcVtc = _marketRepository.GetByCode("BTC_VTC");
            var btcTrx = _marketRepository.GetByCode("BTC_TRX");
            var usdtBtc = _marketRepository.GetByCode("USDT_BTC");
            var usdtLtc = _marketRepository.GetByCode("USDT_LTC");

            _exchangeMarketRepository.Add(new ExchangeMarket
            {
                ExchangeId = binance.Id,
                MarketId = btcLtc.Id,
                ExchangeMarketCode = "LTCBTC"
            });
            _exchangeMarketRepository.Add(new ExchangeMarket
            {
                ExchangeId = bittrex.Id,
                MarketId = btcLtc.Id,
                ExchangeMarketCode = "BTC-LTC"
            });

            _exchangeMarketRepository.Add(new ExchangeMarket
            {
                ExchangeId = binance.Id,
                MarketId = btcPivx.Id,
                ExchangeMarketCode = "PIVXBTC"
            });
            _exchangeMarketRepository.Add(new ExchangeMarket
            {
                ExchangeId = bittrex.Id,
                MarketId = btcPivx.Id,
                ExchangeMarketCode = "BTC-PIVX"
            });
            _exchangeMarketRepository.Add(new ExchangeMarket
            {
                ExchangeId = bittrex.Id,
                MarketId = btcVtc.Id,
                ExchangeMarketCode = "BTC-VTC"
            });
            _exchangeMarketRepository.Add(new ExchangeMarket
            {
                ExchangeId = binance.Id,
                MarketId = btcTrx.Id,
                ExchangeMarketCode = "TRXBTC"
            });
            _exchangeMarketRepository.Add(new ExchangeMarket
            {
                ExchangeId = bittrex.Id,
                MarketId = btcTrx.Id,
                ExchangeMarketCode = "BTC-TRX"
            });
            _exchangeMarketRepository.Add(new ExchangeMarket
            {
                ExchangeId = binance.Id,
                MarketId = usdtBtc.Id,
                ExchangeMarketCode = "BTCUSDT"
            });
            _exchangeMarketRepository.Add(new ExchangeMarket
            {
                ExchangeId = bittrex.Id,
                MarketId = usdtBtc.Id,
                ExchangeMarketCode = "USDT-BTC"
            });
            _exchangeMarketRepository.Add(new ExchangeMarket
            {
                ExchangeId = binance.Id,
                MarketId = usdtLtc.Id,
                ExchangeMarketCode = "LTCUSDT"
            });
            _exchangeMarketRepository.Add(new ExchangeMarket
            {
                ExchangeId = bittrex.Id,
                MarketId = usdtLtc.Id,
                ExchangeMarketCode = "USDT-LTC"
            });

            _exchangeCurrencyRepository.Add(new ExchangeCurrency
            {
                CurrencyId = btc.Id,
                ExchangeId = binance.Id,
                ExchangeCurrencyCode = "BTC"
            });
            _exchangeCurrencyRepository.Add(new ExchangeCurrency
            {
                CurrencyId = btc.Id,
                ExchangeId = bittrex.Id,
                ExchangeCurrencyCode = "BTC"
            });

            _exchangeCurrencyRepository.Add(new ExchangeCurrency
            {
                CurrencyId = ltc.Id,
                ExchangeId = binance.Id,
                ExchangeCurrencyCode = "LTC"
            });
            _exchangeCurrencyRepository.Add(new ExchangeCurrency
            {
                CurrencyId = ltc.Id,
                ExchangeId = bittrex.Id,
                ExchangeCurrencyCode = "LTC"
            });

            _exchangeCurrencyRepository.Add(new ExchangeCurrency
            {
                CurrencyId = pivx.Id,
                ExchangeId = binance.Id,
                ExchangeCurrencyCode = "PIVX"
            });
            _exchangeCurrencyRepository.Add(new ExchangeCurrency
            {
                CurrencyId = pivx.Id,
                ExchangeId = bittrex.Id,
                ExchangeCurrencyCode = "PIVX"
            });

            _exchangeCurrencyRepository.Add(new ExchangeCurrency
            {
                CurrencyId = trx.Id,
                ExchangeId = binance.Id,
                ExchangeCurrencyCode = "TRX"
            });
            _exchangeCurrencyRepository.Add(new ExchangeCurrency
            {
                CurrencyId = trx.Id,
                ExchangeId = bittrex.Id,
                ExchangeCurrencyCode = "TRX"
            });

            _exchangeCurrencyRepository.Add(new ExchangeCurrency
            {
                CurrencyId = vtc.Id,
                ExchangeId = bittrex.Id,
                ExchangeCurrencyCode = "VTC"
            });

            _exchangeCurrencyRepository.Add(new ExchangeCurrency
            {
                CurrencyId = usdt.Id,
                ExchangeId = binance.Id,
                ExchangeCurrencyCode = "USDT"
            });
            _exchangeCurrencyRepository.Add(new ExchangeCurrency
            {
                CurrencyId = usdt.Id,
                ExchangeId = bittrex.Id,
                ExchangeCurrencyCode = "USDT"
            });

            return Ok();
        }
    }
}
