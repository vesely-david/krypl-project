using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Infrastructure.Interfaces
{
    public interface ICurrencyRepository : IRepository<Currency>
    {
        Currency GetByCode(string code);
    }
}
