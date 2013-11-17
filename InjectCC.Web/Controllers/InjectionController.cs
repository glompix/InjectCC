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
            Injection latestInjection;
            using (var tx = new UnitOfWork())
            {
                var repository = new MedicationRepository(tx);
                if (medicationId.HasValue)
                    medication = repository.Find(medicationId.Value);
                else
                    medication = repository.GetFirstForUser(WebSecurity.CurrentUserId);

                if (medication == null)
                    return RedirectErrorToAction("You haven't set up any medications yet.", "New", "Medication");

                var injRepository = new InjectionRepository(tx);
                latestInjection = injRepository.GetLatestFor(medication);
            }

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
                using (var tx = new UnitOfWork())
                {
                    model.Injection.InjectionId = Utilities.NewSequentialGUID();
                    model.Injection.UserId = WebSecurity.CurrentUserId;
                    var repository = new InjectionRepository(tx);
                    repository.Add(model.Injection);
                }

                return RedirectToAction("Index");
            }

            using (var tx = new UnitOfWork())
            {
                var repository = new MedicationRepository(tx);
                var medication = repository.Find(model.Injection.MedicationId);
                if (medication == null)
                {
                    return RedirectErrorToAction("You haven't set up any medications yet.", "New", "Medication");
                }
                model.Locations = medication.Locations.ToList();
            }

            return View("Index", model);
        }
        
        //
        // GET: /Injection/Delete/5
 
        public ActionResult Delete(Guid id)
        {
            Injection injection;
            using (var tx = new UnitOfWork())
            {
                var repository = new InjectionRepository(tx);
                injection = repository.GetById(id);
            }
            return View(injection);
        }

        //
        // POST: /Injection/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            using (var tx = new UnitOfWork())
            {
                var repository = new InjectionRepository(tx);
                Injection injection = repository.GetById(id);
                repository.Remove(injection);
            }  
            return RedirectToAction("Index");
        }

        public ActionResult History()
        {
            IList<Injection> injections;
            using (var tx = new UnitOfWork())
            {
                var repository = new InjectionRepository(tx);
                injections = repository.GetAllForUser(WebSecurity.CurrentUserId);
            }
            return View(new HistoryModel { Injections = injections });
        }
    }
}