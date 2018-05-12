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
    }
}
