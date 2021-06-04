using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using PromotionProcessingApp.Models;
using PromotionProcessingApp.Repository;

namespace PromotionProcessingApp
{
    public class PromotionEngine
    {
        private IPromotionRepository promotionRepository;
        private List<Promotion> _promotions = new List<Promotion>();

        public PromotionEngine(IPromotionRepository promotionRepository)
        {
            this.promotionRepository = promotionRepository;
            _promotions = promotionRepository.GetAllPromotions().ToList();
        }


        public Cart CalculateCartTotal(Cart cart)
        {
            Cart result = new Cart();
            result.CartItems = new List<CartItem>();
            result.Id = cart.Id;
            
            if (cart != null && !cart.CartItems.Any())
                return cart;

            var productPromotion = _promotions.OrderBy(p => p.IsBundledPromotion);

            foreach (var promotion in productPromotion)
            {
                if (promotion.IsBundledPromotion == false)
                {
                    var cartItemData = cart.CartItems
                                        .FirstOrDefault(c => c.Product.Id == promotion.Products.First().ProductId);

                    if (cartItemData != null)
                    {
                        int quantity = cartItemData.Quantity;
                        int promotionQuantity = promotion.Products.First().DiscountQuantity;
                        if (quantity >= promotionQuantity)
                        {
                            decimal discountedQuantity = decimal.Floor(quantity / promotionQuantity);
                            cartItemData.SubTotal = discountedQuantity * promotion.Value;
                            cartItemData.SubTotal += (quantity - (discountedQuantity * promotionQuantity)) * cartItemData.Product.Price;
                            cart.CartItems.First(c => c.Id == cartItemData.Id).IsPromotionApplied = true;
                            result.CartItems.Add(cartItemData);
                        }
                    }
                }
                if (promotion.IsBundledPromotion == true)
                {
                    var cartProducts = cart.CartItems.Where(c => promotion.Products.Any(p => p.ProductId == c.Product.Id && c.Quantity >= p.DiscountQuantity) && c.IsPromotionApplied == false);
                    if (cartProducts != null && cartProducts.Any() && cartProducts.Count() == promotion.Products.Count())
                    {
                        bool IsPromotionApplied = false;
                        foreach (var cartProduct in cartProducts)
                        {
                            int quantity = cartProduct.Quantity;
                            int promotionQuantity = promotion.Products.First(c => c.ProductId == cartProduct.Product.Id).DiscountQuantity;
                            if (!IsPromotionApplied)
                            {
                                cartProduct.SubTotal = promotion.Value;
                            }
                            cartProduct.IsPromotionApplied = IsPromotionApplied = true;
                            result.CartItems.Add(cartProduct);
                        }
                    }
                }
            }

            var nonDiscountedCartProducts = cart.CartItems.Where(c => c.IsPromotionApplied == false);
            if (nonDiscountedCartProducts != null && nonDiscountedCartProducts.Any())
            {
                foreach (var nonDiscountedCartProduct in nonDiscountedCartProducts)
                {
                    nonDiscountedCartProduct.SubTotal = nonDiscountedCartProduct.Product.Price * nonDiscountedCartProduct.Quantity;
                    result.CartItems.Add(nonDiscountedCartProduct);
                }
            }

            AddSubTotal(result);

            return result;
        }

        private static void AddSubTotal(Cart cart)
        {
            if (cart != null && cart.CartItems.Any())
                cart.Total = cart.CartItems.Sum(p => p.SubTotal);
        }
    }
}
