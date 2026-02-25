using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OrderSystem.Domain.Entities;

namespace OrderSystem.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task<Order?> GetByIdAsync(Guid id);
        Task UpdateAsync(Order order);
    }
}