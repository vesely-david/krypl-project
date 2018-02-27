using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Enums
{
    public enum TradeState
    {
        New,
        NewCancelPending,
        NewCanceled,
        PartialyFulfilled,
        PartialyFulfilledCancelPending,
        PartialyFulfilledCanceled,
        Fulfilled
    }
}
