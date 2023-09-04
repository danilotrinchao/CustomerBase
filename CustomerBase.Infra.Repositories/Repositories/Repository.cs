
using CustomerBase.Core.Domain;
using CustomerBase.Core.Domain.Entities;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace CustomerBase.Infra.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly string _connectionString;

        public Repository(IOptions<ConnectionStringOptions> connectionOptions)
        {
            _connectionString = connectionOptions.Value.DefaultConnection;
        }
        public async Task<T> GetById(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = $"SELECT * FROM {typeof(T).Name}s WHERE Id = @Id";
                return await connection.QueryFirstOrDefaultAsync<T>(query, new { Id = id });

            }
           
        }

        public async Task<List<T>> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = $"SELECT * FROM {typeof(T).Name}s";
                return (List<T>)await connection.QueryAsync<T>(query);
            }
            
        }


        public string ParseFilterExpression(Expression<Func<T, bool>> filter)
        {
         
            return filter.Body.ToString();
        }
    }
}
