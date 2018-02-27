using DataLayer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class ExchangeSecret : IdEntity
    {
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }

        public int ExchangeId { get; set; }
        public virtual Exchange Exchange { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

    }
}
