using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TestLibrary;

namespace Server_Designer.Model
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        void Create(TEntity item);
        TEntity FindById(int id);
        IEnumerable<TEntity> Get();
        IEnumerable<TEntity> Get(Func<TEntity, bool> predicate);
        void Remove(TEntity item);
        void Update(TEntity item);
        void Update(Expression<Func<TEntity, bool>> filter,
    IEnumerable<object> updatedSet, // Updated many-to-many relationships
    IEnumerable<object> availableSet, // Lookup collection
    string propertyName);
        IEnumerable<TEntity> GetWithInclude(params Expression<Func<TEntity, object>>[] includeProperties);
        IEnumerable<TEntity> GetWithInclude(Func<TEntity, bool> predicate,
                params Expression<Func<TEntity, object>>[] includeProperties);
        IList CreateList(Type type);
    }
}
