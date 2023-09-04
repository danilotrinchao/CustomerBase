using CustomerBase.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerBase.Core.Domain.Port
{
    public interface IClientRepository: IRepository<Client>
    {
        Client GetClientByEmail(string email);
        Guid InsertClient(Guid Id, string name, string email, string logo);
        void UpdateClient(Guid clientId, string name, string email, string logo);
        void DeleteClient(Guid clientId, bool deleteAddresses);
    }
}
