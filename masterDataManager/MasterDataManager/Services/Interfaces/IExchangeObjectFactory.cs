﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterDataManager.Services.Interfaces
{
    public interface IExchangeObjectFactory
    {
        IExchangeService GetExchange(string exchangeName);
    }
}
