using CustomerBase.Core.Domain.Aggregates;
using CustomerBase.Core.Domain.Entities;
using CustomerBase.Core.Domain.Port;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerBase.Infra.Persistence.Repositories
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        private readonly string _connectionString;
        public AddressRepository(IOptions<ConnectionStringOptions> connectionOptions) : base(connectionOptions) 
        {
            _connectionString = connectionOptions.Value.DefaultConnection;
        }

        public void InsertAddress(Guid Id, string country, string state, string city, string neighborhood, string road, string number, string complement, Guid? clientId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new
                {
                    Id = Id,
                    Country = country,
                    State = state,
                    City = city,
                    Neighborhood = neighborhood,
                    Road = road,
                    Number = number,
                    Complement = complement,
                    ClientId = clientId
                };

                connection.Execute("InsertAddress", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void UpdateAddress(Guid clientId, string country, string state, string city, string neighborhood, string road, string number, string complement)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new
                {
                    ClientId = clientId,
                    Country = country,
                    State = state,
                    City = city,
                    Neighborhood = neighborhood,
                    Road = road,
                    Number = number,
                    Complement = complement,
                };

                connection.Execute("UpdateAddress", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void DeleteAddress(Guid addressId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new
                {
                    AddressId = addressId
                };

                connection.Execute("DeleteAddress", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public Address GetAddressById(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = $"SELECT * FROM Addresses WHERE Id = @Id";
                return connection.QueryFirstOrDefault<Address>(query, new { Id = id });
            }
        }

        public List<Address> GetAddressesByClient(Guid clientId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var query = connection.Query<Address>("SELECT * FROM Addresses WHERE ClientId = @ClientId", new { ClientId = clientId }).ToList();
                return query;
            }
        }

    }
}
