using PromotionProcessingApp.Models;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace PromotionProcessingApp.Tests
{
    public class PromotionEngineTest
    {
        private PromotionEngine _promotionEngine = new PromotionEngine();

        [Fact]
        public void ReturnsCartTotalForNonDiscountedProducts()
        {
            var products = GetProductsFromCart();

            decimal result = _promotionEngine.CalculateCartTotal(products);

            result.Should().Be(115m);
        }

        private List<Product> GetProductsFromCart()
        {
            return this.GetAllProducts();
        }

        private List<Product> GetAllProducts()
        {
            return new List<Product>()
            {
                new Product('A', 50m),
                new Product('B', 30m),
                new Product('C', 20m),
                new Product('D', 15m),
            };
        }

    }
}
