using System;
using DataLayer.Infrastructure.Interfaces;
using DataLayer.Models;

namespace DataLayer.Infrastructure
{
    public class EvaluationRepository : Repository<EvaluationTick>, IEvaluationRepository
    {
        public EvaluationRepository(MasterDataContext dbContext) : base(dbContext)
        {
        }
    }
}
