using MasterDataManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterDataManager.Services
{
    public class MarketDataService : IMarketDataService
    {
        public Dictionary<string, string> GetCurrencyTranslations(string exchange)
        {
            throw new NotImplementedException();
        }
    }
}
