using System;
namespace MasterDataManager.Services.ServiceModels
{
    public class Result
    {
        public bool Success { get; set; }
        public object Data { get; set; }

        public Result(bool success, object data)
        {
            Success = success;
            Data = data;
        }
    }
}
