using DataLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class ExchangeCurrency : IdEntity
    {
        public string ExchangeId { get; set; }
        public Exchange Exchange { get; set; }
        public string CurrencyId { get; set; }
        public Currency Currency { get; set; }
    }
}
