using DataLayer.Enums;
using DataLayer.Infrastructure.Interfaces;
using DataLayer.Models;
using MasterDataManager.Models;
using MasterDataManager.Services.Interfaces;
using MasterDataManager.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterDataManager.Services
{
    public class BalanceService : IBalanceService
    {
        private IUserAssetRepository _userAssetRepository;

        public BalanceService(
            IUserAssetRepository userAssetRepository)
        {
            _userAssetRepository = userAssetRepository;
        }

        public Result UpdateUserAssets(string userId, IEnumerable<Asset> assets, TradingMode tradingMode)
        {
            var userAssets = _userAssetRepository.GetByUserId(userId).Where(o => o.TradingMode == tradingMode);
            foreach(var asset in assets)
            {

                if (!string.IsNullOrEmpty(asset.Id))
                {
                    var original = userAssets.FirstOrDefault(o => o.Id == asset.Id);
                    if(original == null) return new Result { Success = false, Message = "Asset not found" };
                    if (original.StrategyAssets.Any() && original.StrategyAssets.Sum(o => o.Amount) > asset.Amount)
                    {
                        return new Result { Success = false, Message = "Asset locked" };
                    }
                    if (asset.Amount == 0)
                    {
                        _userAssetRepository.DeleteNotSave(original);
                    }
                    else
                    {
                        original.Amount = asset.Amount;
                        _userAssetRepository.EditNotSave(original);
                    }
                }
                else
                {
                    _userAssetRepository.AddNotSave(new UserAsset
                    {
                        UserId = userId,
                        TradingMode = tradingMode,
                        Amount = asset.Amount,
                        Currency = asset.Currency,
                        Exchange = asset.Exchange,
                        StrategyAssets = new List<StrategyAsset>(),
                    });
                }
            }
            _userAssetRepository.Save();
            return new Result { Success = true };
        }
    }
}
