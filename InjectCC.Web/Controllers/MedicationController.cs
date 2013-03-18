using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InjectCC.Web.ViewModels.Medication;
using InjectCC.Model;
using WebMatrix.WebData;
using InjectCC.Web.Filters;

namespace InjectCC.Web.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class MedicationController : InjectCcController
    {
        //
        // GET: /Medication/

        public ActionResult New()
        {
            using (var db = new Context())
            {
                var medication = new Medication();
                var medications = db.Medications.Where(m => m.UserId == WebSecurity.CurrentUserId).ToList();
                var model = NewModel.FromEntity(medication, medications);
                return View(model);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(NewModel model)
        {
            if (ModelState.IsValid)
            {
                var medication = new Medication()
                {
                    UserId = WebSecurity.CurrentUserId,
                    Name = model.Name,
                    Description = model.Description
                };

                using (var db = new Context())
                {
                    db.Medications.Add(medication);
                    db.SaveChanges();
                }

                return RedirectToAction("Edit", new { id = medication.MedicationId });
            }

            using (var db = new Context())
            {
                model.Medications = db.Medications.Where(m => m.UserId == WebSecurity.CurrentUserId).ToList();
            }
            return View(model);
        }
    }
}
