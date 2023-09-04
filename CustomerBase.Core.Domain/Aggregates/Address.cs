using CustomerBase.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerBase.Core.Domain.Aggregates
{
    public class Address: BaseEntity
    {
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Neighborhood { get; set; }
        public string Road { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }

        public Guid? ClientId { get; set; }
    }
}
