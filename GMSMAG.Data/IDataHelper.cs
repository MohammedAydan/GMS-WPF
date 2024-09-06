using System.Collections.Generic;
using System.Threading.Tasks;

namespace GMSMAG.Data
{
    public interface IDataHelper<T>
    {
        // Read Operations
        Task<List<T>> GetAllAsync(int page = 1, int limit = 50);
        Task<List<T>> SearchAsync(string term, string colName = "Id", int page = 1, int limit = 50);
        Task<T> FindAsync(int id);

        // Write Operations
        Task AddAsync(T entity);
        Task AddBulkAsync(List<T> entities);
        Task EditAsync(T entity);
        Task DeleteAsync(int id);
        Task DeleteBulkAsync(List<int> ids);
    }
}
