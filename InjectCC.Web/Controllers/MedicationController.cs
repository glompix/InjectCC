using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InjectCC.Web.ViewModels.Medication;
using InjectCC.Model;
using WebMatrix.WebData;
using InjectCC.Web.Filters;
using System.IO;
using InjectCC.Model.Repositories;
using InjectCC.Model.Domain;
using InjectCC.Model.EntityFramework;
using AutoMapper;

namespace InjectCC.Web.Controllers
{
    [Authorize]
    public class MedicationController : InjectCcController
    {
        private const string _referenceImagePath = "~/Content/reference-images/";
        private MedicationRepository _medications;

        public MedicationController()
        {
            _medications = new MedicationRepository();
        }

        public ActionResult New()
        {
            return View(new NewModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(NewModel model)
        {
            if (ModelState.IsValid)
            {
                var medication = Mapper.Map<Medication>(model);
                _medications.Add(medication);
                return RedirectToAction("Edit", "Injection");
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            
            var allMedications = _medications.ListAllForUser(WebSecurity.CurrentUserId);
            var medication = allMedications.Single(m => m.MedicationId == id);
                
            var model = EditModel.FromEntity(medication, allMedications.ToList());

            model.ReferenceImages = (from f in Directory.GetFiles(Server.MapPath(_referenceImagePath), "*.jpg")
                                     select Url.Content(Path.Combine(_referenceImagePath, Path.GetFileName(f)))).ToList();
            return View(model);
        }
 
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(EditModel model)
        {
            var repository = new MedicationRepository();
            if (ModelState.IsValid)
            {
                var medication = repository.Find(model.MedicationId);
                medication.Name = model.Name;
                medication.Description = model.Description;

                    foreach (var location in model.Locations.Where(l => l.LocationId == default(int)))
                    {
                        medication.AddLocation(location);
                    }
                    foreach (var location in medication.Locations.Where(l => !model.Locations.Any(vml => vml.LocationId == l.LocationId)))
                    {
                        medication.RemoveLocation(location);
                    }

                return RedirectToAction("Edit", new { id = medication.MedicationId });
            }

            model.EditableMedications = repository.ListAllForUser(WebSecurity.CurrentUserId);
            model.ReferenceImages = (from f in Directory.GetFiles(Server.MapPath(_referenceImagePath), "*.jpg")
                                     select Url.Content(Path.Combine(_referenceImagePath, Path.GetFileName(f)))).ToList();
            return View(model);
        }
    }
}
