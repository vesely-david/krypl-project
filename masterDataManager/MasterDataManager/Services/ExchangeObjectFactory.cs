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
        private Dictionary<string, IExchangeService> _serviceDictionary = new Dictionary<string, IExchangeService>();

        public ExchangeObjectFactory(IServiceProvider services)
        {
            _serviceDictionary.Add("binance", services.GetService<BinanceService>());
            _serviceDictionary.Add("poloniex", services.GetService<PoloniexService>());
        }

        public IExchangeService GetExchange(string exchangeName)
        {
            var normalized = exchangeName.ToLower();
            if (!_serviceDictionary.ContainsKey(normalized)) return null;
            return _serviceDictionary[normalized];
        }
    }
}
