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

namespace InjectCC.Web.Controllers
{
    [Authorize]
    public class MedicationController : InjectCcController
    {
        public ActionResult New(int? copyFromId = null)
        {
            var meds = new MedicationRepository();
            var locs = new LocationRepository();

            Medication sourceMed = null;
            if (copyFromId.HasValue)
                sourceMed = meds.Find(copyFromId.Value);

            var medication = new Medication(sourceMed);
            var editableMedications = meds.ListAllForUser(WebSecurity.CurrentUserId);
            var copyableMedications = meds.ListAllForUser(0);
            var model = NewModel.FromEntity(medication, editableMedications, copyableMedications);
            model.ReferenceImages = (from f in Directory.GetFiles(Server.MapPath(_referenceImagePath), "*.jpg")
                                        select Url.Content(Path.Combine(_referenceImagePath, Path.GetFileName(f)))).ToList();
            return View(model);
        }
        private const string _referenceImagePath = "~/Content/reference-images/";

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(NewModel model)
        {
            var repository = new MedicationRepository();
            if (ModelState.IsValid)
            {
                using (var tx = new UnitOfWork())
                {
                    var medication = new Medication
                    {
                        UserId = WebSecurity.CurrentUserId,
                        Name = model.Name,
                        Description = model.Description
                    };

                    foreach (var location in model.Locations)
                    {
                        medication.AddLocation(new Location(location));
                    }

                    repository.Add(medication);
                }
                return RedirectToAction("Index", "Injection");
            }

            model.Locations = model.Locations ?? new List<Location>();

            using (var tx = new UnitOfWork())
            {
                model.EditableMedications = repository.ListAllForUser(WebSecurity.CurrentUserId);
                model.CopyableMedications = repository.ListAllForUser(0);
            }

            model.ReferenceImages = (from f in Directory.GetFiles(Server.MapPath(_referenceImagePath), "*.jpg")
                                     select Url.Content(Path.Combine(_referenceImagePath, Path.GetFileName(f)))).ToList();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var repository = new MedicationRepository();
            var allMedications = repository.ListAllForUser(WebSecurity.CurrentUserId);
            var medication = allMedications.Single(m => m.MedicationId == id);
                
            var model = EditModel.FromEntity(medication, allMedications.ToList());

            model.ReferenceImages = (from f in Directory.GetFiles(Server.MapPath(_referenceImagePath), "*.jpg")
                                     select Url.Content(Path.Combine(_referenceImagePath, Path.GetFileName(f)))).ToList();
            return View(model);
        }
 
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(EditModel model)
        {
            if (ModelState.IsValid)
            {
                Medication medication;
                using (var tx = new UnitOfWork())
                {
                    var medRepository = new MedicationRepository(tx);
                    medication = medRepository.Find(model.MedicationId);
                    medication.Name = model.Name;
                    medication.Description = model.Description;

                    var locRepository = new LocationRepository(tx);
                    var locations = locRepository.GetLocationsFor(medication);
                    foreach (var location in model.Locations.Where(l => l.LocationId == default(int)))
                    {
                        location.MedicationId = medication.MedicationId;
                        locRepository.Add(location);
                    }
                    foreach (var location in locations.Where(l => !model.Locations.Any(vml => vml.LocationId == l.LocationId)))
                    {
                        locRepository.Remove(location);
                    }
                }

                return RedirectToAction("Edit", new { id = medication.MedicationId });
            }

            using (var tx = new UnitOfWork())
            {
                var repository = new MedicationRepository(tx);
                model.EditableMedications = repository.ListAllForUser(WebSecurity.CurrentUserId);
            }
            model.ReferenceImages = (from f in Directory.GetFiles(Server.MapPath(_referenceImagePath), "*.jpg")
                                     select Url.Content(Path.Combine(_referenceImagePath, Path.GetFileName(f)))).ToList();
            return View(model);
        }
    }
}
