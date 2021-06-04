using System;
using System.Collections.Generic;
using System.Text;

namespace PromotionProcessingApp.Models
{
    public class Product
    {
        public char Id { get; set; }
        public decimal Price { get; set; }
        public Product(char id, decimal price)
        {
            this.Id = id;
            this.Price = price;
        }
    }

    
}
