﻿using DataLayer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class ExchangeSecret : IdEntity
    {
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }

        public string ExchangeId { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

    }
}
