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

        public AdminController(
            UserManager<User> userManager,
            IExchangeRepository exchangeRepository,
            IMarketRepository marketRepository,
            ICurrencyRepository currencyRepository,
            IUserAssetRepository userAssetRepository)
        {
            _userManager = userManager;
            _exchangeRepository = exchangeRepository;
            _currencyRepository = currencyRepository;
            _marketRepository = marketRepository;
            _userAssetRepository = userAssetRepository;
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
            if(_exchangeRepository.GetByName(exchange.Name) != null)
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
            var userAssets = SeedUserAssetsAsync("kirchjan");

            await users;
            await userAssets;

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("seedAssets")]
        public async Task<ActionResult> SeedUserAssetsAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound();

            var bittrex = _exchangeRepository.GetByName("bittrex");

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
                TradingMode = TradingMode.Real
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
                TradingMode = TradingMode.Real

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
                TradingMode = TradingMode.Real

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
                TradingMode = TradingMode.Real

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
                TradingMode = TradingMode.Real

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
                    UserName = "kirchjan"
                };
                var a = await _userManager.CreateAsync(honza, "kirchjan!Hesl0");
            }

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
                SecondaryCurrency = ltc
            };
            var btcPivx = new Market
            {
                Code = "BTC_PIVX",
                BaseCurrency = btc,
                SecondaryCurrency = pivx
            };
            var btcVtc = new Market
            {
                Code = "BTC_VTC",
                BaseCurrency = btc,
                SecondaryCurrency = vtc
            };
            var btcTrx = new Market
            {
                Code = "BTC_TRX",
                BaseCurrency = btc,
                SecondaryCurrency = trx
            };
            var usdtBtc = new Market
            {
                Code = "USDT_BTC",
                BaseCurrency = usdt,
                SecondaryCurrency = btc
            };
            var usdtLtc = new Market
            {
                Code = "USDT_LTC",
                BaseCurrency = usdt,
                SecondaryCurrency = ltc
            };

            _marketRepository.Add(btcLtc);
            _marketRepository.Add(btcPivx);
            _marketRepository.Add(btcTrx);
            _marketRepository.Add(btcVtc);
            _marketRepository.Add(usdtBtc);
            _marketRepository.Add(usdtLtc);

            var bittrex = _exchangeRepository.GetByName("bittrex");
            var binance = _exchangeRepository.GetByName("binance");

            //bittrex.Markets.ToList().Add(btcLtc);

            return Ok();
        }
    }
}