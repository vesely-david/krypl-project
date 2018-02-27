using DataLayer.Enums;
using DataLayer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Models
{
    public class UserAsset : IdEntity
    {
        public double Amount { get; set; }
        public TradingMode TradingMode { get; set; }

        public int CurrencyId { get; set; }
        public virtual Currency Currency { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int ExchangeId { get; set; }
        public virtual Exchange Exchange { get; set; }
        public List<StrategyAsset> StrategyAssets { get; set; }

        public double GetFreeAmount()
        {
            return Amount - StrategyAssets.Sum(o => o.Amount);
        }
    }
}
