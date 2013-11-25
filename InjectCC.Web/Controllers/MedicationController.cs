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
                return RedirectToAction("Edit", "Medication", new { id = medication.MedicationId });
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var medication = _medications.GetById(id, WebSecurity.CurrentUserId);
            var refImages = getReferenceImagePaths();
            var model = new EditModel(medication, refImages);
            return View(model);
        }
 
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(EditModel model)
        {
            if (ModelState.IsValid)
            {
                var medication = _medications.Find(model.MedicationId);
                Mapper.Map(model, medication);

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
            
            return View(model);
        }

        private const string _referenceImagePath = "~/Content/reference-images/";
        private IEnumerable<string> getReferenceImagePaths()
        {
            return Directory.GetFiles(Server.MapPath(_referenceImagePath), "*.jpg")
                .Select(f => Url.Content(Path.Combine(_referenceImagePath, Path.GetFileName(f))));
        }
    }
}
