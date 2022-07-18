using Flower.Data;
using Flower.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flower.Controllers
{
    [Authorize(Roles = FC.Admin)]
    public class TableController : Controller
    {
        private readonly ApplicationDbContext db;

        public TableController(ApplicationDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Table> objList = db.Table;
            return View(objList);
        }
        //get-create
        public IActionResult Create()
        {
            return View();
        }

        //post-create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Table obj)
        {
            db.Table.Add(obj);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //get-edit
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = db.Table.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //post-edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Table obj)
        {
            if (ModelState.IsValid)
            {
                db.Table.Update(obj);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);

        }

        //get-delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = db.Table.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        //post-delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = db.Table.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            db.Table.Remove(obj);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

    }
}
