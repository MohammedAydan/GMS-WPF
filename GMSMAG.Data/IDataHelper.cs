using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMSMAG.Data
{
    public interface IDataHelper<Table>
    {
        // Read
        Task<List<Table>> GetAllAsync(int Page = 1,int Limit = 50);
        Task<List<Table>> SearchAsync(string Item,string ColName = "Id", int page = 1, int limit = 50);
        Task<Table> FindAsync(int Id);
        // Wirte
        Task AddAsync(Table table);
        Task EditAsync(Table table);
        Task DeleteAsync(int Id);
    }
}
