using MasterDataManager.Services.Interfaces;
using MasterDataManager.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models;
using DataLayer.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using DataLayer.Enums;

namespace MasterDataManager.Services
{
    public class PoloniexService : IExchangeService
    {
        private BinanceWrapper _binanceWrapper;
        private UserManager<User> _userManager;
        private IMarketDataService _marketDataService;
        private IExchangeSecretRepository _exchangeSecretRepository;
        private readonly string _exchangeName  = "poloniex";

        public PoloniexService(
            UserManager<User> userManager,
            IMarketDataService marketDataService,
            IExchangeSecretRepository exchangeSecretRepository)
        {
            _binanceWrapper = new BinanceWrapper();
            _userManager = userManager;
            _marketDataService = marketDataService;
            _exchangeSecretRepository = exchangeSecretRepository;
        }

        public async Task<List<Asset> > GetRealBalances(string userId)
        {
            var assets = new List<Asset>(); // TODO: IResult implementation???
            var userSecret = _exchangeSecretRepository.GetByUserAndExchange(userId, _exchangeName);
            if (userSecret == null) return assets;
        
            var binanceBalances = await _binanceWrapper.GetBalances(userSecret.ApiKey, userSecret.ApiSecret);
            var notNullBalances = binanceBalances.Where(o => Decimal.Parse(o.free) != 0 || Decimal.Parse(o.locked) != 0);

            var translations = await _marketDataService.GetCurrencyTranslationsAsync(_exchangeName);
            foreach (var balance in notNullBalances)
            {
              
                if(translations.ContainsKey(balance.asset))
                {
                    assets.Add(new Asset
                    {
                        Currency = translations[balance.asset],
                        Amount = Decimal.Parse(balance.free) + Decimal.Parse(balance.locked),
                        UserId = userId,
                        Exchange = _exchangeName,
                        TradingMode = TradingMode.Real,
                        IsActive = true,
                    });
                }
            }
            return assets;
        }
    }
}
