using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ProductRepository
    {
        public const int SECONDS_IN_WEEK = 24 * 60 * 60 * 7;

        public List<ShoppingListItem> GetShoppingList(IEnumerable<ProductModel> productsToProduce)
        {
            List<ShoppingListItem> results = new List<ShoppingListItem>();

            using (EVEEntities entities = new DataAccess.EVEEntities())
            {
                IEnumerable<int> profitable = productsToProduce.Where(t => t.IsProfitable).Select(t => t.ProductId);

                foreach (ProductIngredient pi in entities.ProductIngredients.Where(t => profitable.Contains(t.ProductId)))
                {
                    double efficiencyMultiplier = 1 - (pi.Product.MaterialEfficiency / 100);
                    int productQuantity = (int)(Math.Floor((double)SECONDS_IN_WEEK / (double)entities.Products.Single(t => t.ProductId == pi.ProductId).BuildTimeInSeconds) * efficiencyMultiplier);

                    if (results.Any(t => t.Name == pi.Ingredient.IngredientName)) //update
                    {
                        results.Single(t => t.Name == pi.Ingredient.IngredientName).Quantity += productQuantity * pi.Quantity;
                    }
                    else //add
                    {
                        ShoppingListItem item = new ShoppingListItem { Name = pi.Ingredient.IngredientName, Quantity = productQuantity * pi.Quantity, TypeId = pi.Ingredient.TypeId, PriceEach = pi.Ingredient.IngredientCost };
                        results.Add(item);
                    }
                }
            }

            return results;
        }

        public List<ProductModel> GetProductionList()
        {
            List<ProductModel> results = new List<ProductModel>();

            using (EVEEntities entities = new DataAccess.EVEEntities())
            {
                foreach (Product prod in entities.Products)
                {
                    double efficiencyMultiplier = 1 - (prod.MaterialEfficiency / 100);

                    ProductModel pm = new ProductModel()
                    {
                        Name = prod.ProductName,
                        ProductionCountPerWeek = (int)Math.Floor((double)SECONDS_IN_WEEK / (double)prod.BuildTimeInSeconds * prod.OutputCount),
                        Price = prod.SellPrice,
                        Cost = entities.ProductIngredients.Where(t => t.ProductId == prod.ProductId).Select(t => t.Ingredient.IngredientCost * t.Quantity * efficiencyMultiplier).Sum(),
                        QuantityPerProductionRun = prod.OutputCount,
                        TypeId = prod.TypeId,
                        ProductId = prod.ProductId
                    };

                    pm.Ingredients = this.GetShoppingList(new ProductModel[] { pm });

                    results.Add(pm);
                }
            }

            return results;
        }

        public static void RefreshPrices()
        {
            using (EVEEntities entities = new EVEEntities())
            {
                List<int> productTypeIds = entities.Products.ToList().Where(t => t.PriceUpdated == null || (DateTime.Now - t.PriceUpdated.Value).TotalHours >= 4).Select(t => t.TypeId).ToList();
                List<int> ingredientTypeIds = entities.Ingredients.ToList().Where(t => t.CostUpdated == null || (DateTime.Now - t.CostUpdated.Value).TotalHours >= 4).Select(t => t.TypeId).ToList();
                MarketRepository repo = new MarketRepository();

                foreach (int typeId in productTypeIds)
                {
                    ProductRepository.UpdateProductPrice(typeId, entities, repo);
                    entities.SaveChanges();
                }

                foreach (int typeId in ingredientTypeIds)
                {
                    ProductRepository.UpdateIngredientPrice(typeId, entities, repo);
                    entities.SaveChanges();
                }
            }
        }

        private static void UpdateProductPrice(int typeId, EVEEntities entity, MarketRepository repo)
        {
            double? price = repo.GetProductLowSellPrice(typeId);

            if (price.HasValue)
            {
                Product prod = entity.Products.Single(t => t.TypeId == typeId);

                prod.SellPrice = price.Value;
                prod.PriceUpdated = DateTime.Now;
            }
        }

        private static void UpdateIngredientPrice(int typeId, EVEEntities entity, MarketRepository repo)
        {
            double? price = repo.GetProductLowSellPrice(typeId);

            if (price.HasValue)
            {
                Ingredient ingred = entity.Ingredients.Single(t => t.TypeId == typeId);

                ingred.IngredientCost = price.Value;
                ingred.CostUpdated = DateTime.Now;
            }
        }
    }
}
