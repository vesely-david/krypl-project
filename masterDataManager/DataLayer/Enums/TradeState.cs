using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Enums
{
    public enum TradeState
    {
        New,
        NewCanceled,
        PartialyFulfilled,
        PartialyFulfilledCanceled,
        Fulfilled
    }
}
