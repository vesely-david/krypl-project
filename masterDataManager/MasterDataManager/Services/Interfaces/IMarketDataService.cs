﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterDataManager.Services.Interfaces
{
    public interface IMarketDataService
    {
        Task<Dictionary<string, string>> GetCurrencyTranslationsAsync(string exchange);
    }
}