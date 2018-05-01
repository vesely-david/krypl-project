using DataLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class Market : IdEntity
    {
        public string CurrencyId { get; set; }
        public virtual Currency Currency { get; set; }
        public string MarketCurrencyId { get; set; }
        public virtual Currency MarketCurrency { get; set; }
    }
}
