using DataLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class Market : IdEntity
    {
        public string BaseCurrencyId { get; set; }
        public virtual Currency BaseCurrency { get; set; }
        public string MarketCurrencyId { get; set; }
        public virtual Currency MarketCurrency { get; set; }
    }
}
