using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataLayer.Infrastructure.Interfaces
{
    public interface IRepository<T> where T : IdEntity
    {
        T GetById(string id);
        IEnumerable<T> List();
        IEnumerable<T> List(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
        void AddNotSave(T entity);
        void EditNotSave(T entity);
        void DeleteNotSave(T entity);
        void Save();
    }
}