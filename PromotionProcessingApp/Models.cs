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

    public class Cart
    {
        public int Id { get; set; }
        public decimal Total { get; set; }
        public List<CartItem> CartItems { get; set; }
    }

    public class CartItem
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }
        public bool IsPromotionApplied { get; set; }
        public CartItem(int id, Product product,int quantity, decimal subTotal, bool isPromotionApplied = false)
        {
            this.Id = id;
            this.Product = product;
            this.Quantity = quantity;
            this.SubTotal = subTotal;
            this.IsPromotionApplied = isPromotionApplied;
        }
    }

    public class Promotion
    {
        public int Id { get; set; }
        public Enum PromotionType { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public bool IsBundledPromotion { get; set; }
        public List<DiscountProduct> Products { get; set; }
    }

    public class DiscountProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int DiscountQuantity { get; set; }
        public int PromotionId { get; set; }
    }

    public enum PromotionType
    {
        FlatPrice,
        Percentage
    }

}
