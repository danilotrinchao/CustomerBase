using CustomerBase.Core.Domain.Entities;
using CustomerBase.Core.Domain.Port;
using Dapper;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;

namespace CustomerBase.Infra.Persistence.Repositories
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        private readonly string _connectionString;

        public ClientRepository(IOptions<ConnectionStringOptions> connectionOptions) : base(connectionOptions)
        {
            _connectionString = connectionOptions.Value.DefaultConnection;
        }

        public Client GetClientByEmail(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = $"SELECT * FROM {typeof(Client).Name}s WHERE Email = @email";
                return connection.QueryFirstOrDefault<Client>(query, new { email = email });
            }
        }

        public Guid InsertClient(Guid id, string name, string email, string logo)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new { Id = id, Name = name, Email = email, Logo = logo };
                var clientId = connection.ExecuteScalar<Guid>("InsertClient", parameters, commandType: CommandType.StoredProcedure);
                return clientId;
            }
        }

        public void UpdateClient(Guid clientId, string name, string email, string logo)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new
                {
                    ClientId = clientId,
                    NewName = name,
                    NewEmail = email,
                    NewLogo = logo
                };

                connection.Execute("UpdateClient", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void DeleteClient(Guid clientId, bool deleteAddresses)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new
                {
                    ClientId = clientId,
                    DeleteAddresses = deleteAddresses ? 1 : 0
                };

                connection.Execute("DeleteClient", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }

}
