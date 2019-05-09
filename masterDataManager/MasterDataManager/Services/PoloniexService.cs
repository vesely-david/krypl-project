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
        private PoloniexWrapper _poloniexWrapper;
        private UserManager<User> _userManager;
        private IMarketDataService _marketDataService;
        private ITradeRepository _tradeRepository;
        private ITradeFinalizationService _tradeFinalizationService;
        private IExchangeSecretRepository _exchangeSecretRepository;
        private readonly string _exchangeName  = "poloniex";

        public PoloniexService(
            UserManager<User> userManager,
            IMarketDataService marketDataService,
            ITradeRepository tradeRepository,
            ITradeFinalizationService tradeFinalizationService,
            IExchangeSecretRepository exchangeSecretRepository
            )
        {
            _poloniexWrapper = new PoloniexWrapper();
            _userManager = userManager;
            _marketDataService = marketDataService;
            _exchangeSecretRepository = exchangeSecretRepository;
            _tradeRepository = tradeRepository;
            _tradeFinalizationService = tradeFinalizationService;
        }

        public async Task<List<Asset> > GetRealBalances(string userId)
        {
            var assets = new List<Asset>();
            var userSecret = _exchangeSecretRepository.GetByUserAndExchange(userId, _exchangeName);
            if (userSecret == null) return assets;
        
            var binanceBalances = await _poloniexWrapper.GetBalances(userSecret.ApiKey, userSecret.ApiSecret);

            var translations = await _marketDataService.GetCurrencyTranslationsAsync(_exchangeName);
            foreach (var balance in binanceBalances)
            {
              
                if(translations.ContainsKey(balance.asset))
                {
                    assets.Add(new Asset
                    {
                        Currency = translations[balance.asset],
                        Amount = balance.amount,
                        UserId = userId,
                        Exchange = _exchangeName,
                        TradingMode = TradingMode.Real,
                        IsActive = true,
                    });
                }
            }
            return assets;
        }

        public async Task<string> PutOrder(TradeOrder order, OrderType orderType, string userId)
        {
            var userSecret = _exchangeSecretRepository.GetByUserAndExchange(userId, _exchangeName);
            try
            {
                return await _poloniexWrapper.PutOrder(userSecret.ApiKey, userSecret.ApiSecret, order, orderType);
            }
            catch { return null; }
        }

        public async Task<bool> CancelOrder(string tradeId, string userId)
        {
            var userSecret = _exchangeSecretRepository.GetByUserAndExchange(userId, _exchangeName);
            try
            {
                await _poloniexWrapper.CancelOrder(userSecret.ApiKey, userSecret.ApiSecret, tradeId);
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> MirrorTrades(string userId)
        {
            var userSecret = _exchangeSecretRepository.GetByUserAndExchange(userId, _exchangeName);
            var opened = await _poloniexWrapper.GetOpenedTrades(userSecret.ApiKey, userSecret.ApiSecret);
            var openedTrades = _tradeRepository.GetByUserId(userId).Where(o => o.Strategy.TradingMode == TradingMode.Real && (
            o.TradeState == TradeState.New || o.TradeState == TradeState.PartialyFulfilled) && o.Exchange == _exchangeName);

            foreach (var t in openedTrades.Where(o => !opened.Any(p => o.ExchangeUuid == p)))
            {
                _tradeFinalizationService.ExecuteTrade(t, t.Price);
            }
            return true;
        }
    }
}
