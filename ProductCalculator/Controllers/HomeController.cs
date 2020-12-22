using DataAccess;
using Microsoft.AspNetCore.Mvc;
using ProductCalculator.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCalculator.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            UoW unitOfWork = new UoW();

            Dictionary<string, int> list = unitOfWork.GetShoppingList();

            return View();
        }
    }
}
