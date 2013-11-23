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
        public ActionResult Index(int? medicationId = null)
        {
            Medication medication;
            var meds = new MedicationRepository();
            if (medicationId.HasValue)
                medication = meds.Find(medicationId.Value);
            else
                medication = meds.GetFirstForUser(WebSecurity.CurrentUserId);

            if (medication == null)
                return RedirectErrorToAction("You haven't set up any medications yet.", "New", "Medication");

            var injRepository = new InjectionRepository();
            var latestInjection = injRepository.GetLatestFor(medication);

            Injection nextInjection;
            if (latestInjection == null)
                nextInjection = medication.CalculateFirst();
            else
                nextInjection = latestInjection.CalculateNext();

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
                var injections = new InjectionRepository();
                injections.Add(model.Injection);

                return RedirectToAction("Index");
            }

            var meds = new MedicationRepository();
            var medication = meds.Find(model.Injection.MedicationId);
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
            var injection = new InjectionRepository().GetById(id);
            return View(injection);
        }

        //
        // POST: /Injection/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var repository = new InjectionRepository();
            var injection = repository.GetById(id);
            repository.Remove(injection);  
            return RedirectToAction("Index");
        }

        public ActionResult History()
        {
            var injections = new InjectionRepository().GetAllForUser(WebSecurity.CurrentUserId);
            return View(new HistoryModel { Injections = injections });
        }
    }
}