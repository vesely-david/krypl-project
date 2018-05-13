﻿using DataLayer.Enums;
using DataLayer.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.Models
{
    public class Strategy : IdEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
        public StrategyState StrategyState { get; set; }
        public TradingMode TradingMode { get; set; }
        public int NewTrades { get; set; }

        public string ExchangeId { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<EvaluationTick> Evaluations { get; set; }
        public virtual ICollection<StrategyAsset> StrategyAssets { get; set; }
        public virtual ICollection<Trade> Trades { get; set; }

        //public virtual IEnumerable<SomeError> Errors { get; set; } in case of error (insufficient funds)



        public decimal Get24HoursBtcChange()
        {
            return 1m;
        }
        public decimal Get24HoursUsdChange()
        {
            return 1m;
        }
    }
}