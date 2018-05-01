using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketDataProvider.Services.Models
{
    public class Tick
    {
        public string Market { get; set; }
        public string Currency { get; set; }
        public decimal Price { get; set; }
    }
}
