using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Infrastructure
{
    public class ValuePairRepository : Repository<EvaluationTick>
    {
        public ValuePairRepository(MasterDataContext dbContext) : base(dbContext)
        {
        }
    }
}
