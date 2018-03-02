using MasterDataManager.Services.Interfaces;
using MasterDataManager.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterDataManager.Services
{
    public class BinanceService
    {
        private IBinanceWrapper _binanceWrapper;
        private IExchangeDataProvider _exchangeDataProvider;
        private readonly string _exchangeName  = "binance";

        public BinanceService(
            IBinanceWrapper binanceWrapper,
            IExchangeDataProvider exchangeDataProvider)
        {
            _binanceWrapper = binanceWrapper;
            _exchangeDataProvider = exchangeDataProvider;
        }

        public async Task<List<Asset> > GetBalances()
        {
            var binanceBalances = await _binanceWrapper.GetBalances();
            var assets = new List<Asset>();

            var binanceMarkets = _exchangeDataProvider.GetExchangeMarkets(_exchangeName);

            foreach(var balance in binanceBalances)
            {

            }
            return assets;
        }
    }
}
