using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IMDBScraper.DAL.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetAsync(string id);
        Task InsertAsync(TEntity entity);
        Task InsertAsync(IEnumerable<TEntity> entities);
        Task<bool> DeleteAsync(TEntity entityToDelete);
        Task<bool> DeleteAsync(string id);
        Task<bool> UpdateAsync(TEntity entity);
    }
}
