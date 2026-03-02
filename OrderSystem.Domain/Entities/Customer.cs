using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Domain.Entities
{
    public class Customer : Person
    {
        public Guid Id { get; private set; }

        public Customer(string name, string email) : base(name, email)
        {
            this.Id = Guid.NewGuid();
        }

    }
}
