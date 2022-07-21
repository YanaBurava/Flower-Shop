using Flower.Data;
using Flower.Models;
using Flower.Models.ViewModels;
using Flower.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Flower.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment  _webHostEnvironment;
        private readonly IEmailSender _emailSender;
        [BindProperty]
        public ProductUserViewModel productUserViewModel { get; set; }
        public CartController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            List<ShopingCart> shoppingCartList = new();
            if (HttpContext.Session.Get<IEnumerable<ShopingCart>>(FC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShopingCart>>(FC.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<IEnumerable<ShopingCart>>(FC.SessionCart).ToList();
            }

            List<int> prodInCart = shoppingCartList.Select(i => i.ProductId).ToList();
            IEnumerable<Product> productsList = _db.Product.Where(u => prodInCart.Contains(u.Id));
            return View(productsList);
        }
        public IActionResult Remove(int id)
        {
            List<ShopingCart> shoppingCartList = new();
            if (HttpContext.Session.Get<IEnumerable<ShopingCart>>(FC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShopingCart>>(FC.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<IEnumerable<ShopingCart>>(FC.SessionCart).ToList();
            }
            shoppingCartList.Remove(shoppingCartList.FirstOrDefault(u => u.ProductId == id));
            HttpContext.Session.Set(FC.SessionCart, shoppingCartList);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {          
            return RedirectToAction(nameof(Summary));
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            List<ShopingCart> shoppingCartList = new();
            if (HttpContext.Session.Get<IEnumerable<ShopingCart>>(FC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShopingCart>>(FC.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<IEnumerable<ShopingCart>>(FC.SessionCart).ToList();
            }

            List<int> prodInCart = shoppingCartList.Select(i => i.ProductId).ToList();
            IEnumerable<Product> productsList = _db.Product.Where(u => prodInCart.Contains(u.Id));

            productUserViewModel = new()
            {
                ApplicationUser = _db.AppUser.FirstOrDefault(u => u.Id == claim.Value),
                ProductList = productsList.ToList()
          
            };

            return View(productUserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task <IActionResult> SummaryPost(ProductUserViewModel productUserViewModel)
        {
            var PathToTemplate = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                  + "template" + Path.DirectorySeparatorChar.ToString() +
                  "Inquiry.html";
            var subject = "New";
            string HtmlBody = "";
            using (StreamReader sr = System.IO.File.OpenText(PathToTemplate))
            {
                HtmlBody = sr.ReadToEnd();
            }
            StringBuilder productListSB = new StringBuilder();
            foreach (var prod in productUserViewModel.ProductList)
            {
                productListSB.Append($" - Name: { prod.Name} <span style='font-size:14px;'> (ID: {prod.Id})</span><br />");
            }

            string messageBody = string.Format(HtmlBody,
                productUserViewModel.ApplicationUser.FullName,
                productUserViewModel.ApplicationUser.Email,
                productUserViewModel.ApplicationUser.PhoneNumber,
                productListSB.ToString());

            await _emailSender.SendEmailAsync(FC.AdminEmail, subject, messageBody);
                      
            return RedirectToAction(nameof(InquiryConfirm));
        }

        public IActionResult InquiryConfirm()
        {
            HttpContext.Session.Clear();
            return View();
        }
    }    
}
