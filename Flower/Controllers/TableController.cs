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
        private readonly ApplicationDbContext _db;

        public TableController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Table> objList = _db.Table;
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
            _db.Table.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        //get-edit
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.Table.Find(id);
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
                _db.Table.Update(obj);
                _db.SaveChanges();
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
            var obj = _db.Table.Find(id);
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
            var obj = _db.Table.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Table.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
