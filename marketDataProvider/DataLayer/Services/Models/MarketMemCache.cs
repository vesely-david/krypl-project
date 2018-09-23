using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Services.Models
{
    public class MarketMemCache
    {
        public string Id { get; set; }
        public string CurrencyId { get; set; }
        public string MarketCurrencyId { get; set; }

        public string MarketExchangeId { get; set; }
    }
}
