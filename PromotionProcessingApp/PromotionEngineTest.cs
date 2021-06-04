using PromotionProcessingApp.Models;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace PromotionProcessingApp.Tests
{
    public class PromotionEngineTest
    {
        private PromotionEngine _promotionEngine = new PromotionEngine();

        [Fact(DisplayName = "Scenario 1")]
        public void ReturnsCartTotalForNonDiscountedProducts()
        {
            var products = GetProductsFromCart();

            decimal result = _promotionEngine.CalculateCartTotal(products);

            result.Should().Be(115m);
        }

        [Fact]
        public void ReturnsCartTotalForMultipleQuantitiesOfSameProduct()
        {
            var products = GetMultipleQuantitiesOfSameProductFromCart();

            decimal result = _promotionEngine.CalculateCartTotal(products);

            result.Should().Be(100m);
        }

        [Fact(DisplayName = "Scenario 2")]
        public void ReturnsCartTotalForDiscountedProducts()
        {
            var products = GetProductsHavingDiscountFromCart();

            decimal result = _promotionEngine.CalculateCartTotal(products);

            result.Should().Be(320m);
        }


        private List<Product> GetProductsHavingDiscountFromCart()
        {
            return new List<Product>()
            {
                new Product('A', 50m),
                new Product('A', 50m),
                new Product('A', 50m),
                new Product('A', 50m),
                new Product('A', 50m),
                new Product('B', 30m),
                new Product('B', 30m),
                new Product('B', 30m),
                new Product('B', 30m),
                new Product('B', 30m),
                new Product('C', 20m),
            };
        }

        private List<Product> GetMultipleQuantitiesOfSameProductFromCart()
        {
            return new List<Product>()
            {
                new Product('A', 50m),
                new Product('A', 50m),
            };
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
