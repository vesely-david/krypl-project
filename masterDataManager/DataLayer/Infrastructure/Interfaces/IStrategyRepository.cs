using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Infrastructure.Interfaces
{
    public interface IStrategyRepository : IRepository<Strategy>
    {
        IEnumerable<Strategy> GetByUserId(int userId);
        IEnumerable<Strategy> GetAllForEvaluation();
    }
}
