using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InjectCC.Model;

namespace InjectCC.Web.Controllers
{ 
    public class LocationSetController : Controller
    {
        private InjectionContext db = new InjectionContext();

        //
        // GET: /LocationSet/

        public ViewResult Index()
        {
            var locationsets = db.LocationSets.Include(l => l.User);
            return View(locationsets.ToList());
        }

        //
        // GET: /LocationSet/Details/5

        public ViewResult Details(int id)
        {
            LocationSet locationset = db.LocationSets.Find(id);
            return View(locationset);
        }

        //
        // GET: /LocationSet/Create

        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email");
            return View();
        } 

        //
        // POST: /LocationSet/Create

        [HttpPost]
        public ActionResult Create(LocationSet locationset)
        {
            if (ModelState.IsValid)
            {
                db.LocationSets.Add(locationset);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", locationset.UserId);
            return View(locationset);
        }
        
        //
        // GET: /LocationSet/Edit/5
 
        public ActionResult Edit(int id)
        {
            LocationSet locationset = db.LocationSets.Find(id);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", locationset.UserId);
            return View(locationset);
        }

        //
        // POST: /LocationSet/Edit/5

        [HttpPost]
        public ActionResult Edit(LocationSet locationset)
        {
            if (ModelState.IsValid)
            {
                db.Entry(locationset).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", locationset.UserId);
            return View(locationset);
        }

        //
        // GET: /LocationSet/Delete/5
 
        public ActionResult Delete(int id)
        {
            LocationSet locationset = db.LocationSets.Find(id);
            return View(locationset);
        }

        //
        // POST: /LocationSet/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            LocationSet locationset = db.LocationSets.Find(id);
            db.LocationSets.Remove(locationset);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}