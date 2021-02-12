using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using TestLibrary;

namespace Server_Designer.Model
{
    public class EFGenericRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        DbContext _context;
        DbSet<TEntity> _dbSet;

        public EFGenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public IEnumerable<TEntity> Get()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            return _dbSet.AsNoTracking().Where(predicate).ToList();
        }
        public TEntity FindById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Create(TEntity item)
        {
            _dbSet.Add(item);
            //_context.SaveChanges();
        }
        public void Update(TEntity item)
        {
            if (item == null)
            {
                throw new ArgumentException("Cannot add a null entity.");
            }

            var entry = _context.Entry<TEntity>(item);

            if (entry.State == EntityState.Detached)
            {
                var set = _context.Set<TEntity>();
                TEntity attachedEntity = set.Local.SingleOrDefault(e => e.Id == item.Id);  // You need to have access to key

                if (attachedEntity != null)
                {
                    var attachedEntry = _context.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(item);
                }
                else
                {
                    entry.State = EntityState.Modified; // This should attach entity
                }
                //_dbSet.Attach(item);
                //_context.Entry(item).State = EntityState.Modified;
                // _context.SaveChanges();
            }
        }
        public void Remove(TEntity item)
        {
            //_dbSet.Remove(item);
            //_context.SaveChanges();
            _dbSet.Attach(item);
            _context.Entry(item).State = EntityState.Deleted;
            // _context.SaveChanges();
        }




        public IEnumerable<TEntity> GetWithInclude(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Include(includeProperties).ToList();
        }

        public IEnumerable<TEntity> GetWithInclude(Func<TEntity, bool> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = Include(includeProperties);
            return query.Where(predicate).ToList();
        }

        private IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            return includeProperties
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
        public virtual void Update(Expression<Func<TEntity, bool>> filter,
    IEnumerable<object> updatedSet, // Updated many-to-many relationships
    IEnumerable<object> availableSet, // Lookup collection
    string propertyName) // The name of the navigation property
        {
            // Get the generic type of the set
            var type = updatedSet.GetType().GetGenericArguments()[0];

            // Get the previous entity from the database based on repository type
            var previous = _context
                .Set<TEntity>()
                .Include(propertyName)
                .FirstOrDefault(filter);

            /* Create a container that will hold the values of
                * the generic many-to-many relationships we are updating.
                */
            var values = CreateList(type);

            /* For each object in the updated set find the existing
                 * entity in the database. This is to avoid Entity Framework
                 * from creating new objects or throwing an
                 * error because the object is already attached.
                 */
            foreach (var entry in updatedSet
                .Select(obj => (int)obj
                    .GetType()
                    .GetProperty("Id")
                    .GetValue(obj, null))
                .Select(value => _context.Set(type).Find(value)))
            {
                values.Add(entry);
            }

            /* Get the collection where the previous many to many relationships
                * are stored and assign the new ones.
                */
            _context.Entry(previous).Collection(propertyName).CurrentValue = values;
        }
        public IList CreateList(Type type)
        {
            var genericList = typeof(List<>).MakeGenericType(type);
            return (IList)Activator.CreateInstance(genericList);
        }
    }
}

