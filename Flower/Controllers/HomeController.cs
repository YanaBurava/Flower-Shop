using Flower.Data;
using Flower.Models;
using Flower.Models.ViewModels;
using Flower.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Flower.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            HomeViewModel homeVM = new HomeViewModel()
            {
                Products = _db.Product.Include(u => u.Category).Include(u => u.Table),
                Categories = _db.Category,
            };
        
            return View(homeVM);
        }
        public IActionResult Details(int id)
        {
            List<ShopingCart> shopingCartList = new();
            if (HttpContext.Session.Get<IEnumerable<ShopingCart>>(FC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShopingCart>>(FC.SessionCart).Count() > 0)
            {
                shopingCartList = HttpContext.Session.Get<List<ShopingCart>>(FC.SessionCart);
            }
            DetailsVM detailsVM = new DetailsVM()
            {
                Product = _db.Product.Include(u => u.Category).Include(u => u.Table).Where(u => u.Id == id).FirstOrDefault(),
                ExistsInCart = false
            };
            foreach (var item in shopingCartList)
            {
                if (item.ProductId == id)
                {
                    detailsVM.ExistsInCart = true;
                }
            }
            return View(detailsVM);
        }
        [HttpPost, ActionName("Details")]
        public IActionResult DetailsPost(int id)
        {
            List<ShopingCart> shopingCartList = new();
            if(HttpContext.Session.Get<IEnumerable<ShopingCart>>(FC.SessionCart)!=null
                && HttpContext.Session.Get<IEnumerable<ShopingCart>>(FC.SessionCart).Count() > 0)
            {
                shopingCartList = HttpContext.Session.Get<List<ShopingCart>>(FC.SessionCart);
            }
            shopingCartList.Add(new ShopingCart { ProductId = id });
            HttpContext.Session.Set(FC.SessionCart, shopingCartList);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int id)
        {
            List<ShopingCart> shopingCartList = new();
            if (HttpContext.Session.Get<IEnumerable<ShopingCart>>(FC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShopingCart>>(FC.SessionCart).Count() > 0)
            {
                shopingCartList = HttpContext.Session.Get<List<ShopingCart>>(FC.SessionCart);
            }

            var itemToRemove = shopingCartList.SingleOrDefault(r => r.ProductId == id);
            if (itemToRemove != null)
            {
                shopingCartList.Remove(itemToRemove);
            }
          
            HttpContext.Session.Set(FC.SessionCart, shopingCartList);
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
