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
    public class BinanceService : IExchangeService
    {
        private BinanceWrapper _binanceWrapper;
        private UserManager<User> _userManager;
        private IMarketDataService _marketDataService;
        private IExchangeSecretRepository _exchangeSecretRepository;
        private ITradeRepository _tradeRepository;
        private readonly string _exchangeName  = "binance";

        public BinanceService(
            UserManager<User> userManager,
            IMarketDataService marketDataService,
            IExchangeSecretRepository exchangeSecretRepository,
            ITradeRepository tradeRepository)
        {
            _binanceWrapper = new BinanceWrapper();
            _userManager = userManager;
            _marketDataService = marketDataService;
            _exchangeSecretRepository = exchangeSecretRepository;
            _tradeRepository = tradeRepository;
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

        public async Task<string> PutOrder(TradeOrder order, OrderType orderType, string userId)
        {
            var userSecret = _exchangeSecretRepository.GetByUserAndExchange(userId, _exchangeName);
            var translations = await _marketDataService.GetMarketTranslationsAsync(_exchangeName);

            try
            {
                order.Symbol = translations[order.Symbol];
                return await _binanceWrapper.PutOrder(userSecret.ApiKey, userSecret.ApiSecret, order, orderType);
            }
            catch { return null; }
        } 

        public async Task<bool> CancelOrder(string tradeId, string userId)
        {
            var userSecret = _exchangeSecretRepository.GetByUserAndExchange(userId, _exchangeName);
            var translations = await _marketDataService.GetMarketTranslationsAsync(_exchangeName);
            var trade = _tradeRepository.GetById(tradeId);

            try
            {
                await _binanceWrapper.CancelOrder(userSecret.ApiKey, userSecret.ApiSecret, tradeId, translations[trade.MarketId]);
                return true;
            }
            catch { return false;}
        }

        public Task<bool> MirrorTrades(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
