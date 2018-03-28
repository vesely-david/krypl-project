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

        public async Task<List<Asset> > GetRealBalances(int userId)
        {
            var assets = new List<Asset>(); // IResult implementation???
            var userSecret = _exchangeSecretRepository.GetByUserAndExchange(userId,GetExchangeId());
            if (userSecret == null) return assets;
        
            var binanceBalances = await _binanceWrapper.GetBalances(userSecret.ApiKey, userSecret.ApiSecret);
            var notNullBalances = binanceBalances.Where(o => Double.Parse(o.free) != 0 || Double.Parse(o.locked) != 0);
            foreach (var balance in notNullBalances)
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
                _exchangeId = _exchangeRepository.GetByName(_exchangeName).Id;
            }
            return _exchangeId.Value; //Get Id from staticDataProvider?? 
        }

        public int GetHistoryPrice(Currency currency, DateTime time)
        {
            throw new NotImplementedException();
        }
    }
}
