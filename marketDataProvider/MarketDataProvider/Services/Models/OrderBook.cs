using System;
using System.Collections.Generic;

namespace MarketDataProvider.Services.Models
{
    public class OrderBook
    {
        public List<OrderBookTuple> Asks { get; set; }
        public List<OrderBookTuple> Bids { get; set; }
    }
}
