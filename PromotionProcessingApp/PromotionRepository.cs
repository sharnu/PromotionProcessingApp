using PromotionProcessingApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace PromotionProcessingApp.Repository
{
    public interface IPromotionRepository
    {
        IEnumerable<Promotion> GetAllPromotions();
        IEnumerable<Promotion> GetAllSingleProductPromotions();
        IEnumerable<Promotion> GetAllBundleProductPromotions();
    }

    public class PromotionRepository : IPromotionRepository
    {
        #region Promotion InMemory Data Object
        private readonly List<Promotion> _promotions = new List<Promotion>()
        {
            new Promotion(){ Id= 1, Name ="Promotion-A3", PromotionType = PromotionType.FlatPrice, Value = 130, IsBundledPromotion = false,
            Products = new List<DiscountProduct> {
                    new DiscountProduct(){ Id = 1, ProductId = 'A', DiscountQuantity = 3, PromotionId = 1 },
            } },

            new Promotion(){ Id = 2, Name ="Promotion-B2", PromotionType = PromotionType.FlatPrice, Value = 45, IsBundledPromotion = false,
            Products = new List<DiscountProduct> {
                    new DiscountProduct(){ Id = 2, ProductId = 'B', DiscountQuantity = 2, PromotionId = 2 },
            } },

              new Promotion(){ Id = 3, Name ="Promotion-C&D", PromotionType = PromotionType.FlatPrice, Value = 30, IsBundledPromotion = true,
            Products = new List<DiscountProduct> {
                    new DiscountProduct(){ Id = 3, ProductId = 'C', DiscountQuantity = 1, PromotionId = 3 },
                    new DiscountProduct(){ Id = 4, ProductId = 'D', DiscountQuantity = 1, PromotionId = 3 },
            } }
        };
        #endregion

        public IEnumerable<Promotion> GetAllPromotions()
        {
            return _promotions;
        }

        public IEnumerable<Promotion> GetAllSingleProductPromotions()
        {
            return _promotions.Where(p => p.Products.Count() == 1);
        }
        public IEnumerable<Promotion> GetAllBundleProductPromotions()
        {
            return _promotions.Where(p => p.Products.Count() > 1);
        }
    }
}
