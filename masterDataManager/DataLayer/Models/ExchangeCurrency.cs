using DataLayer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class ExchangeCurrency : IdEntity
    {
        public string ExchangeCurrencyCode { get; set; }

        public int ExchangeId { get; set; }
        public Exchange Exchange { get; set; }
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
    }
}
