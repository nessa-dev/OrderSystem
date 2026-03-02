using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Domain.Entities
{
    public class Person
    {
        public string Name { get; protected set; }
        public string Email { get; protected set; }

        protected Person(string name, string email)
        {
            this.Name = name;
            this.Email = email;
        }

    }
}
