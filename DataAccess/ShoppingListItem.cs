using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ShoppingListItem
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int TypeId { get; set; }
        public double PriceEach { get; set; }
        public double PriceTotal
        {
            get
            {
                return this.Quantity * PriceEach;
            }
        }
    }
}
