using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterDataManager.Services.Interfaces
{
    public interface IExchangeDataProvider
    {
        IEnumerable<Market> GetMarkets(string exchangeName);
        IEnumerable<ExchangeMarket> GetExchangeMarkets(string exchangeName);
        IEnumerable<Currency> GetCurrencies(string exchangeName);
        IEnumerable<ExchangeCurrency> GetExchangeCurrencies(string exchangeName);
        Currency GetCurrency(string exchangeName, string currencyCode);
        Market GetMarket(string exchangeName, string marketCode);
    }
}
