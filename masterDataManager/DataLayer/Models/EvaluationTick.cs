using DataLayer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class EvaluationTick : IdEntity
    {
        public DateTime TimeStamp { get; set; }
        public decimal BtcValue { get; set; }
        public decimal UsdValue { get; set; }
        public bool IsFinal { get; set; }
        public string StrategyId { get; set; }

        public virtual Strategy Strategy { get; set; }

        public EvaluationTick()
        {
            BtcValue = 0;
            UsdValue = 0;
            TimeStamp = DateTime.Now;
            IsFinal = false;
        }
    }
}
