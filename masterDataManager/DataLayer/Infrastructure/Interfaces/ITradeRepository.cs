﻿using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Infrastructure.Interfaces
{
    public interface ITradeRepository : IRepository<Trade>
    {
        Trade GetByUuid(string uuid);
        IEnumerable<Trade> GetByStrategyId(string strategyId);
        IEnumerable<Trade> GetOpenedPaperTrades();
        IEnumerable<Trade> GetByUserId(string userId);

    }
}
