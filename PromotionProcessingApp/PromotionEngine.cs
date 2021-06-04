using PromotionProcessingApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PromotionProcessingApp
{
    public class PromotionEngine
    {
        public decimal CalculateCartTotal(List<Product> products)
        {
            return products.Sum(p => p.Price);
        }
    }
}
