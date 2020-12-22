using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int ProductionCountPerWeek { get; set; }
        public int QuantityPerProductionRun { get; set; }
        public double Price { get; set; }
        public double Cost { get; set; }
        public int TypeId { get; set; }
        public double ProfitPerRun
        {
            get
            {
                return (this.Price - this.Cost);
            }
        }

        public double ProfitPerWeek
        {
            get
            {
                return this.ProfitPerRun * ProductionCountPerWeek;
            }
        }
        public bool IsProfitable
        {
            get
            {
                return this.ProfitPerRun > 0;
            }
        }
        public List<ShoppingListItem> Ingredients { get; set; }
    }
}
