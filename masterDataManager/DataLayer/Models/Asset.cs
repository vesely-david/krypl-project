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

        public virtual User User { get; set; }
        public virtual Strategy Strategy { get; set; 
        }

    }
}
