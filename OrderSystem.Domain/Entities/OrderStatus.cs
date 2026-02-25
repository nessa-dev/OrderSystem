using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Domain.Entities
{
    public enum OrderStatus
    {

        Open = 1,
        Finalized = 2,
        Cancelled = 3
    }
}

