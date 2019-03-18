using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models;

namespace MasterDataManager.Services.Interfaces
{
    public interface IMarketDataService
    {
        Task<Dictionary<string, string>> GetCurrencyTranslationsAsync(string exchange);
        Task<Dictionary<string, (decimal BtcValue, decimal UsdValue)>> GetCurrentPrices(string exchange);
        Task<EvaluationTick> EvaluateAssetSet(IEnumerable<(string currency, decimal amount)> assets, string exchange);
    }
}
