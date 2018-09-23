using DataLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class ExchangeMarket : IdEntity
    {
        public string MarketExchangeId{ get; set; }

        public string ExchangeId { get; set; }
        public Exchange Exchange { get; set; }
        public string MarketId { get; set; }
        public Market Market { get; set; }
    }
}
