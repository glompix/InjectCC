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

namespace InjectCC.Web.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class MedicationController : InjectCcController
    {
        //
        // GET: /Medication/

        public ActionResult New(int? copyFromId = null)
        {
            using (var db = new Context())
            {
                Medication medication;
                IList<Location> locations;
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


                var medications = db.Medications.Where(m => m.UserId == WebSecurity.CurrentUserId).ToList();
                var model = NewModel.FromEntity(medication, locations, medications);
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
                using (var db = new Context())
                {
                    var medication = new Medication
                    {
                        UserId = WebSecurity.CurrentUserId,
                        Name = model.Name,
                        Description = model.Description
                    };
                    db.Medications.Add(medication);

                    foreach (var location in model.Locations)
                    {
                        db.Locations.Add(location);
                    }

                    db.SaveChanges();

                    return RedirectToAction("Edit", new { id = medication.MedicationId });
                }
            }

            using (var db = new Context())
            {
                model.Medications = db.Medications.Where(m => m.UserId == WebSecurity.CurrentUserId).ToList();
            }

            model.ReferenceImages = (from f in Directory.GetFiles(Server.MapPath(_referenceImagePath), "*.jpg")
                                     select Url.Content(Path.Combine(_referenceImagePath, Path.GetFileName(f)))).ToList();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            using (var db = new Context())
            {
                var allMedications = from m in db.Medications
                                     where m.UserId == WebSecurity.CurrentUserId
                                     select m;
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
                model.Medications = db.Medications.Where(m => m.UserId == WebSecurity.CurrentUserId).ToList();
            }
            model.ReferenceImages = (from f in Directory.GetFiles(Server.MapPath(_referenceImagePath), "*.jpg")
                                     select Url.Content(Path.Combine(_referenceImagePath, Path.GetFileName(f)))).ToList();
            return View(model);
        }
    }
}
