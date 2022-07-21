using Flower.Data;
using Flower.Models;
using Flower.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Flower.Controllers
{
    [Authorize(Roles = FC.Admin)]
    public class ProductController : Controller
    {       
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> objList = _db.Product;
            foreach (var obj in objList)
            {
                obj.Category = _db.Category.FirstOrDefault(u => u.Id == obj.CategoryId);
                obj.Table= _db.Table.FirstOrDefault(u => u.Id == obj.TableId);
            };
            return View(objList);
        }
        //get-upsert
        public IActionResult Upsert(int? id)
        {
            ProductViewModel productViewModel = new()
            {
                Product = new Product(),
                CategorySelectList = _db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()

                }),
                TableSelectList = _db.Table.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

        
            if (id == null)
            {
                return View(productViewModel);
            }
            else
            {
                productViewModel.Product = _db.Product.Find(id);
                if (productViewModel.Product == null)
                {
                    return NotFound();
                }
                return View(productViewModel);
            }          
        }

        //post-upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = webHostEnvironment.WebRootPath;

                if(productViewModel.Product.Id == 0)
                {
                    string upload = webRootPath + FC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extention = Path.GetExtension(files[0].FileName);


                    using (var fileStream = new FileStream(Path.Combine (upload, fileName+extention), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    productViewModel.Product.Image = fileName + extention;

                    _db.Product.Add(productViewModel.Product);
                }
                else
                {
                    var objFromBb = _db.Product.AsNoTracking().FirstOrDefault(u => u.Id == productViewModel.Product.Id);
                    if (files.Count > 0)
                    {
                        string upload = webRootPath + FC.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extention = Path.GetExtension(files[0].FileName);
                        var oldFile = Path.Combine(upload, objFromBb.Image);
                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }
                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extention), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        productViewModel.Product.Image = fileName + extention;

                    }
                    else
                    {
                        productViewModel.Product.Image = objFromBb.Image;
                    }
                    _db.Product.Update(productViewModel.Product);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            productViewModel.CategorySelectList = _db.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()

            });
            productViewModel.TableSelectList = _db.Table.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View(productViewModel);           
        }
     
        //get-delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product product = _db.Product.Include(u=>u.Category).Include(u=> u.Table).FirstOrDefault(u=>u.Id==id);
           
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //post-delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Product.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            string upload = webHostEnvironment.WebRootPath + FC.ImagePath;
          
           
            var oldFile = Path.Combine(upload, obj.Image);
            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }

            _db.Product.Remove(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");         
          
        }

    }
}
