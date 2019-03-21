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
        public decimal Quantity { get; set; }
        public decimal QuantityRemaining { get; set; }
        public DateTime Opened { get; set; }
        public DateTime? Closed { get; set; }
        public TradeState TradeState { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public OrderType OrderType { get; set; }

        public string MarketId { get; set; }
        public string StrategyId { get; set; }
        public Strategy Strategy { get; set; }
    }
}
