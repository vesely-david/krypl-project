using MasterDataManager.Services.Interfaces;
using MasterDataManager.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models;
using DataLayer.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace MasterDataManager.Services
{
    public class BinanceService : IHistoryExchangeService, IExchangeService
    {
        private BinanceWrapper _binanceWrapper;
        private UserManager<User> _userManager;
        private IExchangeDataProvider _exchangeDataProvider;
        private IExchangeSecretRepository _exchangeSecretRepository;
        private IExchangeRepository _exchangeRepository;
        private readonly string _exchangeName  = "binance";
        private int? _exchangeId = null;

        public BinanceService(
            UserManager<User> userManager,
            IExchangeDataProvider exchangeDataProvider,
            IExchangeSecretRepository exchangeSecretRepository,
            IExchangeRepository exchangeRepository)
        {
            _binanceWrapper = new BinanceWrapper();
            _userManager = userManager;
            _exchangeDataProvider = exchangeDataProvider;
            _exchangeSecretRepository = exchangeSecretRepository;
            _exchangeRepository = exchangeRepository;
        }

        public async Task<List<Asset> > GetBalances(int userId)
        {
            var assets = new List<Asset>(); // IResult implementation???
            var userSecret = _exchangeSecretRepository.GetByUserAndExchange(userId,GetExchangeId());
            if (userSecret == null) return assets;
        
            var binanceBalances = await _binanceWrapper.GetBalances(userSecret.ApiKey, userSecret.ApiSecret);

            foreach(var balance in binanceBalances)
            {
                var currency = _exchangeDataProvider.GetCurrency(_exchangeName, balance.asset);
                if(currency != null)
                {
                    assets.Add(new Asset
                    {
                        Currency = currency,
                        CurrencyId = currency.Id,
                        Amount = Double.Parse(balance.free) + Double.Parse(balance.locked)
                    });
                }
            }
            return assets;
        }

        public int GetExchangeId()
        {
            if (!_exchangeId.HasValue)
            {
                var _exchangeId = _exchangeRepository.GetByName(_exchangeName).Id;
            }
            return _exchangeId.Value;
        }

        public int GetHistoryPrice(Currency currency, DateTime time)
        {
            throw new NotImplementedException();
        }
    }
}
