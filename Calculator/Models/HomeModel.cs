using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calculator.Models
{
    public class HomeModel
    {
        public List<ProductModel> Products { get; set; }
        public List<ShoppingListItem> ShoppingList { get; set; }
    }
}