using DataLayer.Enums;
using DataLayer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Models
{
    public class UserAsset : IdEntity
    {
        public decimal Amount { get; set; }
        public TradingMode TradingMode { get; set; }

        public string Currency { get; set; }
        public string UserId { get; set; }
        public string Exchange { get; set; }
        public ICollection<StrategyAsset> StrategyAssets { get; set; }

        public decimal GetFreeAmount()
        {
            return Amount - StrategyAssets.Sum(o => o.Amount);
        }
    }
}
