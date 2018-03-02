using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterDataManager.Services.Interfaces
{
    public interface IExchangeDataProvider
    {
        Exchange GetExchange(string exchangeName);
        IEnumerable<Market> GetMarkets(string exchangeName);
        IEnumerable<ExchangeMarket> GetExchangeMarkets(string exchangeName);
    }
}
