﻿using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Infrastructure.Interfaces
{
    public interface ITradeRepository : IRepository<Trade>
    {
        Trade GetByUuid(string uuid);
        Trade GetLast(int? strategyId);
    }
}
