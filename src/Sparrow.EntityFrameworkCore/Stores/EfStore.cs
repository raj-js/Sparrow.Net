using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sparrow.Core;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Sparrow.EntityFrameworkCore.Stores
{
    public class EfStore<TEntity, TPKey> : IEfStore<TEntity, TPKey> where TEntity : class , IEntity<TPKey>
    {
        public DbContext DbContext { get; private set; }

        private DbSet<TEntity> Set => DbContext.Set<TEntity>();

        public EfStore(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<TEntity> Create(TEntity entity)
        {
            var entry = await Set.AddAsync(entity);
            return entry.Entity;
        }

        public Task<int> Modify(TEntity entity)
        {
            var entry = AttemptAttach(entity);
            entry.State = EntityState.Modified;
            return Task.FromResult(1);
        }

        public Task<IQueryable<TEntity>> All()
        {
            return Task.FromResult(Set.AsQueryable());
        }

        public async Task<TEntity> Find(TPKey id)
        {
            return await Set.FindAsync(id);
        }

        public Task<int> Delete(TEntity entity)
        {
            var entry = AttemptAttach(entity);
            entry.State = EntityState.Deleted;
            return Task.FromResult(1);
        }

        public Task<int> SaveChanges()
        {
            return DbContext.SaveChangesAsync();
        }

        #region privates

        private EntityEntry AttemptAttach(TEntity entity) 
        {
            var entry = DbContext.ChangeTracker
                .Entries<TEntity>()
                .FirstOrDefault(s => s.Entity.Equals(entity));

            if (entry != null)
                return entry;

            return DbContext.Attach(entity);
        }

        #endregion
    }
}
