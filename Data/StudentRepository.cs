using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace StudentManagement.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;

        public GenericRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection Connection => new SqlConnection(_connectionString);

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using var conn = Connection;
            string tableName = typeof(T).Name + "s";
            string sql = $"SELECT * FROM {tableName}";
            Console.WriteLine(sql);
            return await conn.QueryAsync<T>(sql);
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            using var conn = Connection;
            string tableName = typeof(T).Name + "s";
            string sql = $"SELECT * FROM {tableName} WHERE Id=@Id";
            return await conn.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(T entity)
        {
            using var conn = Connection;
            string storedProc = "Insert" + typeof(T).Name;
            var parameters = new DynamicParameters();
            var properties = typeof(T).GetProperties()
                .Where(p => p.Name != "Id"
              && p.Name != typeof(T).Name + "Id"
              && p.Name != "ImageFile"
              && p.GetCustomAttributes(typeof(NotMappedAttribute), true).Length == 0
              && (p.PropertyType.IsValueType || p.PropertyType == typeof(string)));
            foreach (var prop in properties)
            {
                parameters.Add(prop.Name, prop.GetValue(entity));
            }
            return await conn.ExecuteAsync(storedProc, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> UpdateAsync(T entity)
        {
            using var conn = Connection;
            string storedProc = "Update" + typeof(T).Name;
            return await conn.ExecuteAsync(storedProc, entity, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var conn = Connection;
            string storedProc = "Delete" + typeof(T).Name;
            return await conn.ExecuteAsync(storedProc, new { Id = id }, commandType: CommandType.StoredProcedure);
        }
    }
}