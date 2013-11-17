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
            using (var tx = new UnitOfWork())
            {
                var repository = new MedicationRepository(tx);

                Medication medication;
                List<Location> locations;
                if (copyFromId.HasValue)
                {
                    var copyFromMedication = repository.Find(copyFromId.Value);
                    medication = new Medication(copyFromMedication);

                    var copyFromLocations = new LocationRepository(tx).GetLocationsFor(copyFromMedication);
                    locations = new List<Location>();
                    foreach (var copyLoc in copyFromLocations)
                    {
                        locations.Add(new Location(copyLoc));
                    }
                }
                else
                {
                    medication = new Medication();
                    locations = new List<Location>();
                }

                var editableMedications = repository.ListAllForUser(WebSecurity.CurrentUserId);
                var copyableMedications = repository.ListAllForUser(0);
                var model = NewModel.FromEntity(medication, locations, editableMedications, copyableMedications);
                model.ReferenceImages = (from f in Directory.GetFiles(Server.MapPath(_referenceImagePath), "*.jpg")
                                         select Url.Content(Path.Combine(_referenceImagePath, Path.GetFileName(f)))).ToList();
                return View(model);
            }
        }
        private const string _referenceImagePath = "~/Content/reference-images/";

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(NewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var tx = new UnitOfWork())
                {
                    var medication = new Medication
                    {
                        UserId = WebSecurity.CurrentUserId,
                        Name = model.Name,
                        Description = model.Description,
                        Locations = model.Locations
                    };

                    var repository = new MedicationRepository(tx);
                    repository.Add(medication);
                }
                return RedirectToAction("Index", "Injection");
            }

            model.Locations = model.Locations ?? new List<Location>();

            using (var tx = new UnitOfWork())
            {
                var repository = new MedicationRepository(tx);
                model.EditableMedications = repository.ListAllForUser(WebSecurity.CurrentUserId);
                model.CopyableMedications = repository.ListAllForUser(0);
            }

            model.ReferenceImages = (from f in Directory.GetFiles(Server.MapPath(_referenceImagePath), "*.jpg")
                                     select Url.Content(Path.Combine(_referenceImagePath, Path.GetFileName(f)))).ToList();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            EditModel model;
            using (var tx = new UnitOfWork())
            {
                var repository = new MedicationRepository(tx);
                var allMedications = repository.ListAllForUser(WebSecurity.CurrentUserId);
                var medication = allMedications.Single(m => m.MedicationId == id);
                
                var locRepository = new LocationRepository(tx);
                var locations = locRepository.GetLocationsFor(medication);
                model = EditModel.FromEntity(medication, locations.ToList(), allMedications.ToList());
            }

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
