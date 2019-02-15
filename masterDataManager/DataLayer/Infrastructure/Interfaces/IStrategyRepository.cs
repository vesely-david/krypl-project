using DataLayer.Enums;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Infrastructure.Interfaces
{
    public interface IStrategyRepository : IRepository<Strategy>
    {
        IEnumerable<Strategy> GetByUserId(string userId);
        IEnumerable<Strategy> GetAllForEvaluation();
        IEnumerable<Strategy> GetUserStrategiesByMode(string userId, TradingMode mode);
        Strategy GetUserOverviewByMode(string userId, TradingMode mode);
    }
}
