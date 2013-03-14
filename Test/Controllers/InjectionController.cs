using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InjectCC.Model;
using InjectCC.Web.ViewModels.Injection;

namespace InjectCC.Web.Controllers
{ 
    [Authorize]
    public class InjectionController : Controller
    {
        private InjectionContext db = new InjectionContext();

        public ActionResult Index(int? id = null)
        {
            var locationSet = (from l in db.LocationSets
                               where l.UserId != null && (id == null || id == l.LocationSetId) // TODO: UserId == loggedInUser
                               select l).FirstOrDefault();

            if (locationSet == null)
            {
                TempData["Error"] = "You haven't set up any medications yet.";
                return RedirectToAction("Settings", "User");
            }

            var latestInjection = (from i in db.Injections
                                   join l in db.Locations on i.LocationId equals l.LocationId into joined
                                   where locationSet.LocationSetId == joined.Single().LocationSetId
                                   orderby i.Date descending
                                   select i).FirstOrDefault();

            Injection nextInjection;
            if (latestInjection == null)
            {
                nextInjection = new Injection();
            }
            else
            {
                nextInjection = latestInjection.CalculateNext();
            }
            var model = new IndexModel
            {
                NextInjection = nextInjection,
                Locations = locationSet.Locations,
                LocationModifiers = locationSet.LocationModifiers,
                Last30DaysRating = 80, // TODO
                Last90DaysRating = 90
            };

            return View(model);
        }

        public ActionResult History()
        {
            return View();
        }










        //
        // POST: /Injection/Create

        [HttpPost]
        public ActionResult Create(Injection injection)
        {
            if (ModelState.IsValid)
            {
                db.Injections.Add(injection);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", injection.UserId);
            return View(injection);
        }
        
        //
        // GET: /Injection/Edit/5
 
        public ActionResult Edit(Guid id)
        {
            Injection injection = db.Injections.Find(id);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", injection.UserId);
            return View(injection);
        }

        //
        // POST: /Injection/Edit/5

        [HttpPost]
        public ActionResult Edit(Injection injection)
        {
            if (ModelState.IsValid)
            {
                db.Entry(injection).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", injection.UserId);
            return View(injection);
        }

        //
        // GET: /Injection/Delete/5
 
        public ActionResult Delete(Guid id)
        {
            Injection injection = db.Injections.Find(id);
            return View(injection);
        }

        //
        // POST: /Injection/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {            
            Injection injection = db.Injections.Find(id);
            db.Injections.Remove(injection);
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