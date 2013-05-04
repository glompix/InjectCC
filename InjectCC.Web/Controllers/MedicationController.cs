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

namespace InjectCC.Web.Controllers
{
    [InitializeSimpleMembership]
    [Authorize]
    public class MedicationController : InjectCcController
    {
        private MedicationRepository _repository;
        private Context _context;
        public MedicationController(MedicationRepository repository)
        {
            _context = new Context();
            _repository = repository;
        }

        public MedicationController()
        {
            // TODO: Think I see why an IoC container makes sense now.
            _context = new Context();
            _repository = new MedicationRepository(_context);
        }

        public ActionResult New(int? copyFromId = null)
        {
            using (var db = new Context())
            {
                Medication medication;
                List<Location> locations;
                if (copyFromId.HasValue)
                {
                    var copyFromMedication = (from m in db.Medications
                                             where m.MedicationId == copyFromId.Value
                                             select m).Single();
                    medication = new Medication(copyFromMedication);

                    var copyFromLocations = from l in db.Locations
                                            where l.MedicationId == copyFromId.Value
                                            select l;
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

                var editableMedications = db.Medications.Where(m => m.UserId == WebSecurity.CurrentUserId).ToList();
                var copyableMedications = db.Medications.Where(m => m.UserId == 0).ToList();
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
                var medication = new Medication
                {
                    UserId = WebSecurity.CurrentUserId,
                    Name = model.Name,
                    Description = model.Description,
                    Locations = model.Locations
                };
                _repository.Create(medication);
                _context.SaveChanges();
                return RedirectToAction("Index", "Injection");
            }

            model.Locations = model.Locations ?? new List<Location>();

            using (var db = new Context())
            {
                model.EditableMedications = db.Medications.Where(m => m.UserId == WebSecurity.CurrentUserId).ToList();
                model.CopyableMedications = db.Medications.Where(m => m.UserId == 0).ToList();
            }

            model.ReferenceImages = (from f in Directory.GetFiles(Server.MapPath(_referenceImagePath), "*.jpg")
                                     select Url.Content(Path.Combine(_referenceImagePath, Path.GetFileName(f)))).ToList();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            using (var db = new Context())
            {
                var allMedications = _repository.ListAllForUser(WebSecurity.CurrentUserId);
                var medication = allMedications.Single(m => m.MedicationId == id);
                var locations = from l in db.Locations
                                where l.MedicationId == id
                                select l;

                var model = EditModel.FromEntity(medication, locations.ToList(), allMedications.ToList());
                model.ReferenceImages = (from f in Directory.GetFiles(Server.MapPath(_referenceImagePath), "*.jpg")
                                         select Url.Content(Path.Combine(_referenceImagePath, Path.GetFileName(f)))).ToList();
                return View(model);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(EditModel model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new Context())
                {
                    var medication = db.Medications.Single(m => m.MedicationId == model.MedicationId);
                    medication.Name = model.Name;
                    medication.Description = model.Description;

                    var locations = (from l in db.Locations
                                    where l.MedicationId == model.MedicationId
                                    select l).ToList();
                    foreach (var location in model.Locations.Where(l => l.LocationId == default(int)))
                    {
                        location.MedicationId = medication.MedicationId;
                        db.Locations.Add(location);
                    }
                    foreach (var location in locations.Where(l => !model.Locations.Any(vml => vml.LocationId == l.LocationId)))
                    {
                        db.Locations.Remove(location);
                    }

                    db.SaveChanges();

                    return RedirectToAction("Edit", new { id = medication.MedicationId });
                }
            }

            using (var db = new Context())
            {
                model.EditableMedications = db.Medications.Where(m => m.UserId == WebSecurity.CurrentUserId).ToList();
            }
            model.ReferenceImages = (from f in Directory.GetFiles(Server.MapPath(_referenceImagePath), "*.jpg")
                                     select Url.Content(Path.Combine(_referenceImagePath, Path.GetFileName(f)))).ToList();
            return View(model);
        }
    }
}
