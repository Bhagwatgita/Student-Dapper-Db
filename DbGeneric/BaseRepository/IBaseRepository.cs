using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace DbGeneric.BaseRepository
{
    public interface IBaseRepository<T>
    {
        void Insert(T entity);
        void Delete(T entity);
        void Update(T entity);
        IEnumerable<T> Query(string where = null);
        int QueryForCount();
        Task<GridReader> ReceiveBulkData(string query, DynamicParameters param);
        Task<IEnumerable<T>> ReceiveTableData(string query, DynamicParameters param);
        Task<T> ReceiveRowData(string query, DynamicParameters param);
    }
}
