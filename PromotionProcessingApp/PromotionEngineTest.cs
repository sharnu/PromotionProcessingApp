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

            var result = _promotionEngine.CalculateCartTotal(products);

            result.Total.Should().Be(100m);
        }

        [Fact]
        public void ReturnsCartTotalForMultipleQuantitiesOfSameProduct()
        {
            var products = GetMultipleQuantitiesOfSameProductFromCart();

            var result = _promotionEngine.CalculateCartTotal(products);

            result.Total.Should().Be(100m);
        }

        [Fact(DisplayName = "Scenario 2")]
        public void ReturnsCartTotalForDiscountedProducts()
        {
            var cart = GetProductsHavingDiscountFromCart();

            var result = _promotionEngine.CalculateCartTotal(cart);

            result.Total.Should().Be(370m);
        }


        private Cart GetProductsHavingDiscountFromCart()
        {
            List<CartItem> cartItem = new List<CartItem>();
            cartItem.Add(new CartItem(1, new Product('A', 50m), 5, 0m));
            cartItem.Add(new CartItem(2, new Product('B', 30m), 5, 0m));
            cartItem.Add(new CartItem(3, new Product('C', 20m), 1, 0m));

            Cart cart = new Cart();
            cart.CartItems = cartItem;
            cart.Id = 1;
            cart.Total = 0;

            return cart;
        }

        private Cart GetMultipleQuantitiesOfSameProductFromCart()
        {
            List<CartItem> cartItem = new List<CartItem>();
            cartItem.Add(new CartItem(1, new Product('A', 50m), 2, 0m));

            Cart cart = new Cart();
            cart.CartItems = cartItem;
            cart.Id = 1;
            cart.Total = 0;

            return cart;
        }

        private Cart GetProductsFromCart()
        {
            return this.GetAllProducts();
        }

        private Cart GetAllProducts()
        {
            List<CartItem> cartItem = new List<CartItem>();
            cartItem.Add(new CartItem(1, new Product('A', 50m), 1, 0m));
            cartItem.Add(new CartItem(2, new Product('B', 30m), 1, 0m));
            cartItem.Add(new CartItem(3, new Product('C', 20m), 1, 0m));

            Cart cart = new Cart();
            cart.CartItems = cartItem;
            cart.Id = 1;
            cart.Total = 0;

            return cart;

        }

    }
}
