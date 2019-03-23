using DataLayer.Enums;
using DataLayer.Infrastructure;

namespace DataLayer.Models
{
    public class Asset : IdEntity
    {
        public decimal Amount { get; set; }
        public TradingMode TradingMode { get; set; }

        public string Currency { get; set; }
        public string Exchange { get; set; }
        public string UserId { get; set; }
        public string StrategyId { get; set; }
        public bool IsActive { get; set; } //Asset is inactivate after strategy is stopped

        public virtual User User { get; set; }
        public virtual Strategy Strategy { get; set; }

        public Asset(Asset asset)
        {
            Amount = asset.Amount;
            TradingMode = asset.TradingMode;
            Currency = asset.Currency;
            Exchange = asset.Exchange;
            UserId = asset.UserId;
            IsActive = true;
        }
        public Asset()
        {
            IsActive = true;
        }

    }
}
