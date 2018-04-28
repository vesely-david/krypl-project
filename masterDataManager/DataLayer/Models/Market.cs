﻿using DataLayer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class Market : IdEntity
    {
        public string Code { get; set; }

        public int BaseCurrencyId { get; set; }
        public virtual Currency BaseCurrency { get; set; }
        public int MarketCurrencyId { get; set; }
        public virtual Currency MarketCurrency { get; set; }
    }
}