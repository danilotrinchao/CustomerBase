using CustomerBase.Core.Domain.Aggregates;
using CustomerBase.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerBase.Core.ApplicationService.Interfaces
{
    public interface ICustomerBaseService
    {
        Task<List<Client>> GetAll();
        Task<Client> GetById(Guid id);
        Task<Guid> CreateClientWithAddress(Client client);
        Task<bool> UpdateClientWithAddress(Client client);
        Task<bool> DeleteClientWithAddress(Guid id, bool deleteAddress);
        Task<Guid> CreateAddress(Address address);
        Task<bool> UpdateAddress(Guid addressId, string country, string state, string city, string neighborhood, string road, string number, string complement);
        void DeleteAddress(Guid id);
    }
}
