using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InjectCC.Model;

namespace InjectCC.Web.Controllers
{ 
    public class InjectionController : Controller
    {
        private InjectionContext db = new InjectionContext();

        //
        // GET: /Injection/

        public ViewResult Index()
        {
            var injection = db.Injection.Include(i => i.User);
            return View(injection.ToList());
        }

        //
        // GET: /Injection/Details/5

        public ViewResult Details(Guid id)
        {
            Injection injection = db.Injection.Find(id);
            return View(injection);
        }

        //
        // GET: /Injection/Create

        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email");
            return View();
        } 

        //
        // POST: /Injection/Create

        [HttpPost]
        public ActionResult Create(Injection injection)
        {
            if (ModelState.IsValid)
            {
                injection.InjectionId = Guid.NewGuid();
                db.Injection.Add(injection);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", injection.UserId);
            return View(injection);
        }
        
        //
        // GET: /Injection/Edit/5
 
        public ActionResult Edit(Guid id)
        {
            Injection injection = db.Injection.Find(id);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", injection.UserId);
            return View(injection);
        }

        //
        // POST: /Injection/Edit/5

        [HttpPost]
        public ActionResult Edit(Injection injection)
        {
            if (ModelState.IsValid)
            {
                db.Entry(injection).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", injection.UserId);
            return View(injection);
        }

        //
        // GET: /Injection/Delete/5
 
        public ActionResult Delete(Guid id)
        {
            Injection injection = db.Injection.Find(id);
            return View(injection);
        }

        //
        // POST: /Injection/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {            
            Injection injection = db.Injection.Find(id);
            db.Injection.Remove(injection);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}