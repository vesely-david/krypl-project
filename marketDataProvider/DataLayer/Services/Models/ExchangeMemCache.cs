using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Services.Models
{
    public class ExchangeMemCache
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Web { get; set; }
        public bool ProvidesFullHistoryData { get; set; }

        public virtual ICollection<MarketMemCache> Markets { get; set; }
        public virtual ICollection<CurrencyMemCache> Currencies { get; set; }
    }
}