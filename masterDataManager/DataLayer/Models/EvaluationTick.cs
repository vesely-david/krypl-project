using DataLayer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class EvaluationTick : IdEntity
    {
        public DateTime TimeStamp { get; set; }
        public double BtcValue { get; set; }
        public double UsdValue { get; set; }
    }
}
