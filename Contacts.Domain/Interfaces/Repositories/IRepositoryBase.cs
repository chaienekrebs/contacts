using Contacts.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Contacts.Domain.Interfaces.Repositories
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> where);
        void Save(TEntity obj);
        void Update(TEntity obj);
        void Delete(TEntity obj);
        int SaveChanges();
        DbContext Context();
        IQueryable<TEntity> QueryAll();
    }
}
