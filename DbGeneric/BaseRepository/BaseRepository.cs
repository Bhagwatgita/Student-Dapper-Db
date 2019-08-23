using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace DbGeneric.BaseRepository
{
    public abstract class BaseRepository<T>: IBaseRepository<T>
    {
        protected string connectionString = @"server=STPL-PC-GME;database=GenericDB;UId=sa; Password=sasa";

        public virtual void Insert(T entity)
        {
            var columns = GetColumns();
            var stringOfColumns = string.Join(", ", columns);
            var stringOfParameters = string.Join(", ", columns.Select(e => "@" + e));
            var query = $"insert into {typeof(T).Name} ({stringOfColumns}) values ({stringOfParameters})";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Execute(query, entity);
            }
        }

        public virtual void Delete(T entity)
        {
            var query = $"delete from {typeof(T).Name} where Id = @Id";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Execute(query, entity);
            }
        }

        public virtual void Update(T entity)
        {
            var columns = GetColumns();
            var stringOfColumns = string.Join(", ", columns.Select(e => $"{e} = @{e}"));
            var query = $"update {typeof(T).Name} set {stringOfColumns} where Id = @Id";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Execute(query, entity);
            }
        }

        public virtual IEnumerable<T> Query(string where = null)
        {
            var query = $"select * from {typeof(T).Name} ";

            if (!string.IsNullOrWhiteSpace(where))
                query += where;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<T>(query);
            }
        }

        public virtual int QueryForCount()
        {
            string countNeedForWhichTable = typeof(T).Name;
            var query = $"select TotalCount=count(*) from {countNeedForWhichTable} ";

            if (!string.IsNullOrWhiteSpace(countNeedForWhichTable))
                query += countNeedForWhichTable;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.QueryFirst<int>(query);
            }
        }

        private IEnumerable<string> GetColumns()
        {
            return typeof(T)
                    .GetProperties()
                    .Where(e => e.Name != "Id" && !e.PropertyType.GetTypeInfo().IsGenericType)
                    .Select(e => e.Name);
        }
        public async Task<GridReader> ReceiveBulkData(string query, DynamicParameters param)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var reader = await connection.QueryMultipleAsync(query, param: param, commandType: CommandType.StoredProcedure))
            {
                return reader;
            }
        }

        public async Task<IEnumerable<T>> ReceiveTableData(string query, DynamicParameters param)
        {
            
            using (var reader = await ReceiveBulkData(query,param))
            {
                if (reader == null || reader.Read<T>().ToList().Count == 0)
                    return null;

                return reader.Read<T>().ToList();
            }

        }

        public async Task<T> ReceiveRowData(string query, DynamicParameters param)
        {
            
            using (var reader = await ReceiveBulkData(query, param))
            {
                if (reader == null || reader.Read<T>().ToList().Count == 0 || reader.Read<T>().Count() == 0)
                    return default(T);

                return reader.Read<T>().First();
            }

        }
    }
}
