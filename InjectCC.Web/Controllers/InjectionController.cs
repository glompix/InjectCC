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

        public ActionResult Index(int? medicationId = null)
        {
            var medication = (from l in db.Medications
                               where l.UserId == WebSecurity.CurrentUserId && (medicationId == null || medicationId == l.MedicationId)
                               select l).FirstOrDefault();

            if (medication == null)
            {
                return RedirectErrorToAction("You haven't set up any medications yet.", "New", "Medication");
            }

            var latestInjection = (from i in db.Injections
                                   where i.MedicationId == medication.MedicationId
                                   orderby i.Date descending
                                   select i).FirstOrDefault();

            Injection nextInjection;
            if (latestInjection == null)
            {
                nextInjection = new Injection
                {
                    Date = DateTime.Now,
                    MedicationId = medication.MedicationId
                };
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

        [HttpPost]
        public ActionResult Create(IndexModel model)
        {
            if (ModelState.IsValid)
            {
                model.NextInjection.InjectionId = Utilities.NewSequentialGUID();
                model.NextInjection.UserId = WebSecurity.CurrentUserId;
                db.Injections.Add(model.NextInjection);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }
            model.Last30DaysRating = 80;
            model.Last90DaysRating = 90;

            var medication = db.Medications.FirstOrDefault(l => l.UserId == WebSecurity.CurrentUserId && (model.NextInjection.MedicationId == l.MedicationId));
            if (medication == null)
            {
                return RedirectErrorToAction("You haven't set up any medications yet.", "New", "Medication");
            }
            model.Locations = medication.Locations.ToList();
            return View("Index", model);
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

        public ActionResult History()
        {
            var injections = from i in db.Injections
                             where i.UserId == WebSecurity.CurrentUserId
                             select i;
            return View(new HistoryModel { Injections = injections.ToList() });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}