using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Calculator.Models;
using DataAccess;
using YamlDotNet.Serialization;

namespace Calculator.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            MarketRepository marketRepo = new MarketRepository();

            ProductRepository repo = new ProductRepository();
            List<ProductModel> productionList = repo.GetProductionList().OrderByDescending(t => t.ProfitPerWeek).ToList();
            HomeModel model = new HomeModel()
            {
                Products = productionList,
                ShoppingList = repo.GetShoppingList(productionList).OrderBy(t => t.Name).ToList()
            };

            return View(model);
        }

        public ActionResult RefreshProduct(int productId)
        {
            using(EVEEntities entities = new EVEEntities())
            {
                MarketRepository repo = new MarketRepository();
                Product prod = entities.Products.Single(t => t.ProductId == productId);

                prod.SellPrice = repo.GetProductLowSellPrice(prod.TypeId);
                prod.PriceUpdated = DateTime.Now;

                foreach(Ingredient ingred in prod.ProductIngredients.Select(t=> t.Ingredient))
                {
                    if((DateTime.Now - ingred.CostUpdated.GetValueOrDefault()).TotalHours > 4)
                    {
                        ingred.IngredientCost = repo.GetProductLowSellPrice(ingred.TypeId);
                        ingred.CostUpdated = DateTime.Now;
                    }
                }

                entities.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}