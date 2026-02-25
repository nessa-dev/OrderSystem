using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Domain.Entities
{
    public class Order
    {
        private readonly List<Product> _products = new();
        public OrderStatus Status { get; private set; } = OrderStatus.Open;

        public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

        public void AddProduct(Product product)
        {
            if (Status != OrderStatus.Open)
                throw new InvalidOperationException("Cannot add products to a finalized order.");

            _products.Add(product);
        }

        public decimal CalculateTotal()
        {
            return _products.Sum(p => p.Price);

        }
        public void FinalizeOrder()
        {
            if (Status != OrderStatus.Open)
                throw new InvalidOperationException("Only open orders can be finalized.");
            if (!_products.Any())
                throw new InvalidOperationException("Cannot finalize an order with no products.");
            Status = OrderStatus.Finalized;

        }


    }
}

