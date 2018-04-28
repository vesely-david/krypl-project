using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Repositories
{

    public class Repository<T> : IRepository<T> where T : IdEntity
    {
        protected readonly MarketDataContext _dbContext;

        public Repository(MarketDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual T GetById(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public virtual IEnumerable<T> List()
        {
            return _dbContext.Set<T>().AsEnumerable();
        }

        public virtual IEnumerable<T> List(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>()
                   .Where(predicate)
                   .AsEnumerable();
        }

        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();
        }

        public void Edit(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public void AddNotSave(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        public void EditNotSave(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void DeleteNotSave(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
