using Contacts.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Contacts.Helpers;

namespace Contacts.Persistence.Repositories
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        public DbContext _dbContextEntity { get; set; }
        public DbSet<TEntity> _dbSetEntity { get; set; }

        public RepositoryBase(ContactsDbContext dbContext)
        {
            _dbContextEntity = dbContext;
            _dbContextEntity.ChangeTracker.AutoDetectChangesEnabled = false;
            _dbSetEntity = dbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> where)
        {
            try
            {
                return _dbSetEntity.Where(where).AsNoTracking();
            }
            catch
            {
                throw new Exception("There was an issue fetching the data!");
            }
        }

        public IQueryable<TEntity> QueryAll()
        {
            try
            {
                return _dbSetEntity.AsNoTracking();
            }
            catch
            {
                throw new Exception("There was an issue fetching the data!");
            }
        }

        public void Save(TEntity obj)
        {
            try
            {
                _dbSetEntity.Add(obj);
            }
            catch
            {
                throw new Exception("There was an issue while saving the record!");
            }
        }

        public void Update(TEntity obj)
        {
            //_dbContextEntity.ChangeTracker.Clear();
            _dbSetEntity.Update(obj);
        }

        public void Delete(TEntity obj)
        {
            try
            {
                _dbSetEntity.Remove(obj);
            }
            catch
            {
                throw new Exception("There was an issue occurred while deleting this contact!");
            }
        }

        public int SaveChanges()
        {
            var written = 0;
            while (written == 0)
            {
                try
                {
                    written = _dbContextEntity.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        throw new NotSupportedException("Concurrency error in " + entry.Metadata.Name);
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Error while executing the operation!");
                }
            }
            return written;
        }

        public DbContext Context()
        {
            try
            {
                return _dbContextEntity;
            }
            catch (Exception)
            {
                throw new Exception("Database connection error!");
            }
        }
    }
}