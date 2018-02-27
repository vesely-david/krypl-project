using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Infrastructure.Interfaces
{
    public interface IExchangeRepository : IRepository<Exchange>
    {
        Exchange GetByName(string name);
    }
}
