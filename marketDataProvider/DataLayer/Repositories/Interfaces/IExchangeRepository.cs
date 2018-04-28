using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Repositories.Interfaces
{
    public interface IExchangeRepository : IRepository<Exchange>
    {
        IEnumerable<Exchange> GetAllWithCurrencies();
    }
}
