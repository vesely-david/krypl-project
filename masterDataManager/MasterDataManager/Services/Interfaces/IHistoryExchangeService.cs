using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterDataManager.Services.Interfaces
{
    public interface IHistoryExchangeService
    {
        int GetHistoryPrice(Currency currency, DateTime time);
    }
}
