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



    }

    }

