using DataLayer.Enums;
using DataLayer.Models;
using MasterDataManager.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterDataManager.Services
{
    public interface IExchangeService
    {
        Task<List<Asset>> GetRealBalances(string userId);
        //UserAsset GetBalance(Currency currency);
        //string SellOrder(Market market, double quantity, double rate);
        //string BuyOrder(Market market, double quantity, double rate);
        //bool CancelOrder(Trade trade);
        //Trade GetOrder(Trade trade);
    }
}
