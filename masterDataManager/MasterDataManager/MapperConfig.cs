using System;
using System.Linq;
using AutoMapper;
using DataLayer.Models;
using MasterDataManager.Models;
using MasterDataManager.Services.ServiceModels;

namespace MasterDataManager
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Asset, JsonAssetModel>().ReverseMap();

            CreateMap<EvaluationTick, JsonEvaluationModel>().ReverseMap();

            CreateMap<JsonTradeModel, Trade>().ReverseMap();

            CreateMap<Strategy, JsonStrategyModel>()
                .ForMember(dest => dest.exchange,
                    opts => opts.MapFrom(src => src.ExchangeId))
                .ForMember(dest => dest.tradesCount,
                    opts => opts.MapFrom(src => src.Trades.Count()))
                .ForMember(dest => dest.openedTradesCount,
                    opts => opts.MapFrom(src => src.Trades.Count(o => !o.Closed.HasValue)))
                .ForMember(dest => dest.newTradesCount,
                    opts => opts.MapFrom(src => src.Trades.Count(o => o.Closed.HasValue ? o.Closed.Value > src.LastCheck : o.Opened > src.LastCheck)))
                .ForMember(dest => dest.initialValue,
                    opts => opts.MapFrom(src => src.Evaluations.First()))
                .ForMember(dest => dest.currentValue,
                    opts => opts.MapFrom(src => src.Evaluations.Last()))
                .ForMember(dest => dest.yesterdayValue,
                    opts => opts.MapFrom(src => src.GetYesterdayValue()));

            CreateMap<UserAsset, JsonUserAssetModel>()
                .ForMember(dest => dest.free,
                    opts => opts.MapFrom(src => src.GetFreeAmount()));

        }
    }
}
