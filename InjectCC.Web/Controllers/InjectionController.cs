using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InjectCC.Model;
using InjectCC.Web.ViewModels.Injection;
using InjectCC.Web.Filters;
using WebMatrix.WebData;

namespace InjectCC.Web.Controllers
{ 
    [Authorize]
    [InitializeSimpleMembership]
    public class InjectionController : InjectCcController
    {
        private Context db = new Context();

        public ActionResult Index(int? id = null)
        {
            var medication = (from l in db.Medications
                               where l.UserId == WebSecurity.CurrentUserId && (id == null || id == l.MedicationId) // TODO: UserId == loggedInUser
                               select l).FirstOrDefault();

            if (medication == null)
            {
                return RedirectErrorToAction("You haven't set up any medications yet.", "New", "Medication");
            }

            var latestInjection = (from i in db.Injections
                                   where i.UserId == WebSecurity.CurrentUserId
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

            var locations = db.Locations.Where(l => l.MedicationId == medication.MedicationId).ToList();
            var locationModifiers = new List<LocationModifier>();
            var model = new IndexModel
            {
                NextInjection = nextInjection,
                Locations = locations,
                LocationModifiers = locationModifiers,
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