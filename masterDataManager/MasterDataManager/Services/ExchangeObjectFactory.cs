using MasterDataManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;


namespace MasterDataManager.Services
{
    public class ExchangeObjectFactory : IExchangeObjectFactory
    {
        public IServiceProvider Services { get; }
        private readonly Dictionary<string, IExchangeService> _serviceDictionary;

        public ExchangeObjectFactory(IServiceProvider services)
        {
            _serviceDictionary.Add("binance", services.GetService<BinanceService>());
            _serviceDictionary.Add("bittrex", services.GetService<BittrexService>());
        }

        public IExchangeService GetExchange(string exchangeName)
        {
            return _serviceDictionary[exchangeName];
        }
    }
}
