using System.Collections.Generic;
using Xunit;

namespace PromotionProcessingApp
{
    public class PromotionEngineTest
    {
        private PromotionEngine _promotionEngine = new PromotionEngine();

        [Fact]
        public void ReturnsCartTotalForNonDiscountedProducts()
        {
            var products = GetProductsFromCart();

            var result = _promotionEngine.CalculateCartTotal(products);

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
                new Product() { Id = 'A', Price = 50m},
                new Product() { Id = 'B', Price = 30m},
                new Product() { Id = 'C', Price = 20m},
                new Product() { Id = 'D', Price = 15m},
            };
        }

    }
}
