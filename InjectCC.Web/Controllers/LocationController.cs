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
    public class LocationController : Controller
    {
        private InjectionContext db = new InjectionContext();

        //
        // GET: /Location/

        public ViewResult Index()
        {
            var locations = db.Locations.Include(l => l.LocationSet);
            return View(locations.ToList());
        }

        //
        // GET: /Location/Details/5

        public ViewResult Details(int id)
        {
            Location location = db.Locations.Find(id);
            return View(location);
        }

        //
        // GET: /Location/Create

        public ActionResult Create()
        {
            ViewBag.LocationSetId = new SelectList(db.LocationSets, "LocationSetId", "Name");
            return View();
        } 

        //
        // POST: /Location/Create

        [HttpPost]
        public ActionResult Create(Location location)
        {
            if (ModelState.IsValid)
            {
                db.Locations.Add(location);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.LocationSetId = new SelectList(db.LocationSets, "LocationSetId", "Name", location.LocationSetId);
            return View(location);
        }
        
        //
        // GET: /Location/Edit/5
 
        public ActionResult Edit(int id)
        {
            Location location = db.Locations.Find(id);
            ViewBag.LocationSetId = new SelectList(db.LocationSets, "LocationSetId", "Name", location.LocationSetId);
            return View(location);
        }

        //
        // POST: /Location/Edit/5

        [HttpPost]
        public ActionResult Edit(Location location)
        {
            if (ModelState.IsValid)
            {
                db.Entry(location).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocationSetId = new SelectList(db.LocationSets, "LocationSetId", "Name", location.LocationSetId);
            return View(location);
        }

        //
        // GET: /Location/Delete/5
 
        public ActionResult Delete(int id)
        {
            Location location = db.Locations.Find(id);
            return View(location);
        }

        //
        // POST: /Location/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Location location = db.Locations.Find(id);
            db.Locations.Remove(location);
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