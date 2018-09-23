using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketDataProvider.Services.Models
{
    public class BinanceTick
    {
        public string symbol { get; set; }
        public string price { get; set; }
    }
}
