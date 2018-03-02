using DataLayer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class Exchange : IdEntity
    {
        public string Name { get; set; }
        public string Web { get; set; }

        public virtual IEnumerable<ExchangeMarket> ExchangeMarkets { get; set; }
        public virtual IEnumerable<ExchangeCurrency> ExchangeCurrencies { get; set; }
        public virtual IEnumerable<ExchangeSecret> ExchangeSecrets { get; set; }
    }
}
