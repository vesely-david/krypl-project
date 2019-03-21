using DataLayer.Enums;
using DataLayer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Models
{
    public class Strategy : IdEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime? Stop { get; set; }
        public StrategyState StrategyState { get; set; }
        public TradingMode TradingMode { get; set; } //null in case of overview
        public DateTime? LastCheck { get; set; }
        public bool IsOverview { get; set; }

        //public string ExchangeId { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<EvaluationTick> Evaluations { get; set; }
        public virtual ICollection<Asset> Assets { get; set; }
        public virtual ICollection<Trade> Trades { get; set; }

        public int GetNewTrades()
        {
            return Trades.Count(o => o.Closed.HasValue ? o.Closed <= LastCheck : o.Opened <= LastCheck);
        }

        public EvaluationTick GetYesterdayValue()
        {
            return Evaluations.ElementAt(Math.Max(0, Evaluations.Count - 24));
        }
    }
}
