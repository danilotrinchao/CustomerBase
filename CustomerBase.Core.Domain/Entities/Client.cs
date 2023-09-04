using CustomerBase.Core.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerBase.Core.Domain.Entities
{
    public class Client : BaseEntity
    {
        public string NameClient { get; set; }
        public string Email { get; set; }
        public string Logo { get; set; }
        public List<Address> AddressClient { get; set; }

    }
}
