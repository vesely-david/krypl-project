using DataLayer.Enums;
using DataLayer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class Trade : IdEntity
    {
        public string ExchangeUuid { get; set; }
        public double Quantity { get; set; }
        public double QuantityRemaining { get; set; }
        public DateTime Opened { get; set; }
        public DateTime? Closed { get; set; }
        public TradeState TradeState { get; set; }
        public double Price { get; set; }
        public OrderType OrderType { get; set; }

        public int ExchangeId { get; set; }
        public virtual Exchange Exchange { get; set; }
        public int MarketId { get; set; }
        public virtual Market Market { get; set; }
        public int StrategyId { get; set; }
        public Strategy Strategy { get; set; }
    }
}
