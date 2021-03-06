using System;
using System.Collections.Generic;
using System.Linq;
using PromotionProcessingApp.Models;
using PromotionProcessingApp.Repository;

namespace PromotionProcessingApp
{
    public interface IPromotionRule 
    {
        Cart CalculateCartTotal(Cart cart);
        int Priority { get; }
    }

    public interface IPromotionCalculator
    {
        Cart CalculatePromotion(Cart cart);
    }

    public interface IPromotionEngine
    {
        Cart CalculateCartTotal(Cart cart);
    }

    public abstract class BasePromotion
    {
        protected List<Promotion> _promotions = new List<Promotion>();

        public int Priority { get; protected set; }

        protected static void AddSubTotal(Cart cart)
        {
            if (cart != null && cart.CartItems.Any())
                cart.Total = cart.CartItems.Sum(p => p.SubTotal);
        }

        protected static Cart MapCart(Cart cart)
        {
            Cart result = new Cart();
            result.CartItems = new List<CartItem>();
            result.CartItems = cart.CartItems;
            result.Id = cart.Id;
            result.Total = cart.Total;
            return result;
        }

    }

    public class SingleProductFlatPricePromotion : BasePromotion, IPromotionRule
    {
        private readonly IPromotionRepository _promotionRepository;

        public SingleProductFlatPricePromotion(IPromotionRepository promotionRepository)
        {
            _promotionRepository = promotionRepository;
            _promotions.AddRange(_promotionRepository.GetAllSingleProductPromotions());
            Priority = 1;
        }

        public Cart CalculateCartTotal(Cart cart)
        {
            Cart result = MapCart(cart);

            if (cart != null && !cart.CartItems.Any())
                return cart;


            foreach (var promotion in _promotions)
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
                        result.CartItems.Where(c => c.Id == cartItemData.Id).Select(c => { return cartItemData; });
                    }
                }
            }

            AddSubTotal(result);

            return result;
        }

    }

    public class BundledProductsFlatPricePromotion : BasePromotion, IPromotionRule
    {
        private readonly IPromotionRepository _promotionRepository;

        public BundledProductsFlatPricePromotion(IPromotionRepository promotionRepository)
        {
            _promotionRepository = promotionRepository;
            _promotions.AddRange(_promotionRepository.GetAllBundleProductPromotions());
            Priority = 2;
        }

        public Cart CalculateCartTotal(Cart cart)
        {
            Cart result = MapCart(cart);

            if (cart != null && !cart.CartItems.Any())
                return cart;

            foreach (var promotion in _promotions)
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
                        result.CartItems.Where(c => c.Id == cartProduct.Id).Select(c => { return cartProduct; });
                    }
                }
            }

            AddSubTotal(result);

            return result;
        }
    }

    public class NonPromotionProducts : BasePromotion, IPromotionRule
    {
        private readonly IPromotionRepository _promotionRepository;

        public NonPromotionProducts(IPromotionRepository promotionRepository)
        {
            _promotionRepository = promotionRepository;
            Priority = 3;
        }

        public Cart CalculateCartTotal(Cart cart)
        {
            Cart result = MapCart(cart);
            var nonDiscountedCartProducts = cart.CartItems.Where(c => c.IsPromotionApplied == false);
            if (nonDiscountedCartProducts != null && nonDiscountedCartProducts.Any())
            {
                foreach (var nonDiscountedCartProduct in nonDiscountedCartProducts)
                {
                    nonDiscountedCartProduct.SubTotal = nonDiscountedCartProduct.Product.Price * nonDiscountedCartProduct.Quantity;
                    result.CartItems.Where(c => c.Id == nonDiscountedCartProduct.Id).Select(c => { return nonDiscountedCartProduct; });
                }
            }

            AddSubTotal(result);
            return result;
        }

    }

    public class PromotionEngine : IPromotionEngine
    {
        List<IPromotionRule> _promotionRules = new List<IPromotionRule>();

        public PromotionEngine(IEnumerable<IPromotionRule> promotionRules)
        {
            _promotionRules.AddRange(promotionRules);
        }

        public Cart CalculateCartTotal(Cart cart)
        {
            Cart result = null;
            if (cart != null && !cart.CartItems.Any())
                return cart;

            // Order by Priority
            foreach (var promotionRule in _promotionRules.OrderBy(p => p.Priority))
            {
                result = promotionRule.CalculateCartTotal(cart);
            }

            return result;
        }

    }

    public class PromotionCalculator : IPromotionCalculator
    {
        public Cart CalculatePromotion(Cart cart)
        {
            try
            {
                Object[] args = { new PromotionRepository() };

                var ruleType = typeof(IPromotionRule);
                IEnumerable<IPromotionRule> promotionRules = this.GetType().Assembly.GetTypes()
                    .Where(p => ruleType.IsAssignableFrom(p) && !p.IsInterface)
                    .Select(r => Activator.CreateInstance(r, args) as IPromotionRule);

                IPromotionEngine engine = new PromotionEngine(promotionRules);

                return engine.CalculateCartTotal(cart);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }

    
}
