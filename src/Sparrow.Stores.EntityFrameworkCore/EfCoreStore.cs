using Microsoft.EntityFrameworkCore;
using Sparrow.Core;
using Sparrow.Core.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace Sparrow.Stores.EntityFrameworkCore
{
    public class EfCoreStore<TEntity, TKey> :
        ICreateStore<TEntity, TKey>,
        IRemoveStore<TEntity, TKey>,
        IUpdateStore<TEntity, TKey>,
        IQueryStore<TEntity, TKey>,
        IStore<TEntity, TKey>

        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        protected DbContext DbContext { get; private set; }

        protected DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

        public EfCoreStore(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        public long Count()
        {
            return DbSet.LongCount();
        }

        public long Count(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.LongCount(predicate);
        }

        public Task<long> CountAsync()
        {
            return DbSet.LongCountAsync();
        }

        public Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.LongCountAsync(predicate);
        }

        public TEntity Create(TEntity entity)
        {
            return DbSet.Add(entity).Entity;
        }

        public TKey CreateAndGetId(TEntity entity)
        {
            return DbSet.Add(entity).Entity.Id;
        }

        public async Task<TKey> CreateAndGetIdAsync(TEntity entity)
        {
            return (await DbSet.AddAsync(entity)).Entity.Id;
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            return (await DbSet.AddAsync(entity)).Entity;
        }

        public void CreateMany(IEnumerable<TEntity> entities)
        {
            DbSet.AddRange(entities);
        }

        public Task CreateManyAsync(IEnumerable<TEntity> entities)
        {
            return DbSet.AddRangeAsync(entities);
        }

        public TEntity Find(TKey id)
        {
            return DbSet.Find(id);
        }

        public Task<TEntity> FindAsync(TKey id)
        {
            return DbSet.FindAsync(id);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<(IEnumerable<TEntity> Entities, long Total)> PageQuery(Expression<Func<TEntity, bool>> predicate, (string Field, bool IsAsc)[] sortFields, int pageIndex, int pageSize)
        {
            var total = await DbSet.LongCountAsync(predicate);

            var queryable = DbSet.Where(predicate);
            IOrderedQueryable<TEntity> orderedQueryable = null;

            for (var i = 0; i < sortFields.Length; i++)
            {
                var (Field, IsAsc) = sortFields[i];

                orderedQueryable = i == 0 ?
                    queryable.OrderBy($"{Field} {(IsAsc ? "asc" : "desc")}") :
                    orderedQueryable.ThenBy($"{Field} {(IsAsc ? "asc" : "desc")}");
            }

            orderedQueryable = orderedQueryable ?? queryable.OrderBy(s => s.Id);
            var entities = orderedQueryable.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return (entities, total);
        }

        public async Task<(IEnumerable<TEntity> Entities, long Total)> PageQuery(Expression<Func<TEntity, bool>> predicate, (Expression<Func<TEntity, object>> Selector, bool IsAsc)[] sortFields, int pageIndex, int pageSize)
        {
            var total = await DbSet.LongCountAsync(predicate);

            var queryable = DbSet.Where(predicate);
            IOrderedQueryable<TEntity> orderedQueryable = null;

            for (var i = 0; i < sortFields.Length; i++)
            {
                var (Selector, IsAsc) = sortFields[i];

                orderedQueryable = i == 0 ?
                    (IsAsc ? queryable.OrderBy(Selector) : queryable.OrderByDescending(Selector)) :
                    (IsAsc ? orderedQueryable.ThenBy(Selector) : orderedQueryable.ThenByDescending(Selector));
            }

            orderedQueryable = orderedQueryable ?? queryable.OrderBy(s => s.Id);
            var entities = orderedQueryable.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return (entities, total);
        }

        public IQueryable<TEntity> Query()
        {
            return DbSet.AsQueryable();
        }

        public bool Remove(TEntity entity)
        {
            DbSet.Remove(entity);
            return true;
        }

        public bool Remove(TKey id)
        {
            DbSet.Remove(Find(id));
            return true;
        }

        public bool Remove(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = DbSet.Where(predicate);
            DbSet.RemoveRange(entities);
            return true;
        }

        public async Task<bool> RemoveAsync(TEntity entity)
        {
            DbSet.Remove(entity);
            await Task.CompletedTask;
            return true;
        }

        public async Task<bool> RemoveAsync(TKey id)
        {
            DbSet.Remove(Find(id));
            await Task.CompletedTask;
            return true;
        }

        public async Task<bool> RemoveAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = DbSet.Where(predicate);
            DbSet.RemoveRange(await entities.ToListAsync());
            return true;
        }

        public TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.SingleOrDefault(predicate);
        }

        public Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.SingleOrDefaultAsync(predicate);
        }

        public TEntity Update(TEntity entity)
        {
            return DbSet.Update(entity).Entity;
        }

        public TEntity Update(TKey id, Action<TEntity> updateAction)
        {
            var entity = Find(id);
            updateAction?.Invoke(entity);
            return entity;
        }

        public TEntity Update(TKey id, params (string Field, object Value)[] selectors)
        {
            var entity = Find(id);
            var type = typeof(TEntity);

            foreach (var (Field, Value) in selectors)
                type.GetProperty(Field).SetValue(entity, Value);

            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var res = DbSet.Update(entity).Entity;
            await Task.CompletedTask;
            return res;
        }

        public async Task<TEntity> UpdateAsync(TKey id, Func<TEntity, Task> updateAction)
        {
            var entity = Find(id);
            await updateAction?.Invoke(entity);
            return entity;
        }

        public async Task<TEntity> UpdateAsync(TKey id, params (string Field, object Value)[] selectors)
        {
            var entity = Find(id);
            var type = typeof(TEntity);

            foreach (var (Field, Value) in selectors)
                type.GetProperty(Field).SetValue(entity, Value);

            await Task.CompletedTask;

            return entity;
        }

        public void Save()
        {
            DbContext.SaveChanges();
        }

        public Task SaveAsync()
        {
            return DbContext.SaveChangesAsync();
        }
    }
}
