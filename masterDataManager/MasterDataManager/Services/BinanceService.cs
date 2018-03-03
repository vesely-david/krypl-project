using MasterDataManager.Services.Interfaces;
using MasterDataManager.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models;
using DataLayer.Infrastructure.Interfaces;

namespace MasterDataManager.Services
{
    public class BinanceService : IHistoryExchangeService, IExchangeService
    {
        private BinanceWrapper _binanceWrapper;
        private IExchangeDataProvider _exchangeDataProvider;
        private IExchangeRepository _exchangeRepository;
        private readonly string _exchangeName  = "binance";

        public BinanceService(
            IExchangeDataProvider exchangeDataProvider,
            IExchangeRepository exchangeRepository)
        {
            _binanceWrapper = new BinanceWrapper();
            _exchangeDataProvider = exchangeDataProvider;
            _exchangeRepository = exchangeRepository;
        }

        public async Task<List<Asset> > GetBalances()
        {
            var binanceBalances = await _binanceWrapper.GetBalances();
            var assets = new List<Asset>();

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
            var exchange = _exchangeRepository.GetByName(_exchangeName);
            return exchange == null ? -1 : exchange.Id;
        }

        public int GetHistoryPrice(Currency currency, DateTime time)
        {
            throw new NotImplementedException();
        }
    }
}
