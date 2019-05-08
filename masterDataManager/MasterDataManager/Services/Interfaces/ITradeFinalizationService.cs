using System;
using DataLayer.Models;
using MasterDataManager.Services.ServiceModels;

namespace MasterDataManager.Services.Interfaces
{
    public interface ITradeFinalizationService
    {

        Result ExecuteTrade(Trade trade, decimal rate);
        Result CancelTrade(Trade trade);
    }
}
