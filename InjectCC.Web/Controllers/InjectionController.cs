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
using InjectCC.Model.Domain;
using InjectCC.Model.EntityFramework;

namespace InjectCC.Web.Controllers
{ 
    [Authorize]
    public class InjectionController : InjectCcController
    {
        private Context db = new Context();

        public ActionResult Index(int? medicationId = null)
        {
            var repository = new MedicationRepository(db);
            Medication medication;
            if (medicationId.HasValue)
                medication = repository.Find(medicationId.Value);
            else
                medication = repository.GetFirstForUser(WebSecurity.CurrentUserId);

            if (medication == null)
                return RedirectErrorToAction("You haven't set up any medications yet.", "New", "Medication");

            var injRepository = new InjectionRepository(db);
            var latestInjection = injRepository.GetLatestFor(medication);

            Injection nextInjection;
            if (latestInjection == null)
            {
                nextInjection = new Injection
                {
                    Date = DateTime.Now,
                    MedicationId = medication.MedicationId,
                    Location = medication.Locations.First()
                };
            }
            else
            {
                nextInjection = latestInjection.CalculateNext();
            }

            var model = new IndexModel
            {
                Injection = nextInjection,
                Locations = medication.Locations
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(IndexModel model)
        {
            if (ModelState.IsValid)
            {
                model.Injection.InjectionId = Utilities.NewSequentialGUID();
                model.Injection.UserId = WebSecurity.CurrentUserId;
                var repository = new InjectionRepository(db);
                repository.Add(model.Injection);
                return RedirectToAction("Index");  
            }

            var medication = db.Medications.FirstOrDefault(l => l.UserId == WebSecurity.CurrentUserId && (model.Injection.MedicationId == l.MedicationId));
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
            var repository = new InjectionRepository(db);
            Injection injection = repository.GetById(id);
            return View(injection);
        }

        //
        // POST: /Injection/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var repository = new InjectionRepository(db);
            Injection injection = repository.GetById(id);
            repository.Remove(injection();
            .
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