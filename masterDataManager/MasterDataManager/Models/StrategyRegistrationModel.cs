using System;
using System.Collections.Generic;
using DataLayer.Enums;

namespace MasterDataManager.Models
{
    public class StrategyRegistrationModel
    {
        public string name { get; set; }
        public string exchange { get; set; }
        public string description { get; set; }
        public TradingMode tradingMode { get; set; }
        public IEnumerable<JsonAssetModel> assets { get; set; }
    }
}
