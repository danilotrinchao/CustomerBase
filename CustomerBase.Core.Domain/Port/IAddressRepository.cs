using CustomerBase.Core.Domain.Aggregates;
using CustomerBase.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerBase.Core.Domain.Port
{
    public interface IAddressRepository : IRepository<Address>
    {
        void InsertAddress(Guid addressId, string country, string state, string city, string neighborhood, string road, string number, string complement, Guid? clientId);
        List<Address> GetAddressesByClient(Guid clientId);
        void UpdateAddress(Guid clientId, string country, string state, string city, string neighborhood, string road, string number, string complement);
        void DeleteAddress(Guid addressId);
        Address GetAddressById(Guid id);

    }
}
