using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;

namespace BugTracker.Controllers
{
    public class AllowedFileTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AllowedFileTypes
        public ActionResult Index()
        {
            return View(db.AllowedFileTypes.ToList());
        }

        // GET: AllowedFileTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AllowedFileType allowedFileType = db.AllowedFileTypes.Find(id);
            if (allowedFileType == null)
            {
                return HttpNotFound();
            }
            return View(allowedFileType);
        }

        // GET: AllowedFileTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AllowedFileTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Type")] AllowedFileType allowedFileType)
        {
            if (ModelState.IsValid)
            {
                db.AllowedFileTypes.Add(allowedFileType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(allowedFileType);
        }

        // GET: AllowedFileTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AllowedFileType allowedFileType = db.AllowedFileTypes.Find(id);
            if (allowedFileType == null)
            {
                return HttpNotFound();
            }
            return View(allowedFileType);
        }

        // POST: AllowedFileTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Type")] AllowedFileType allowedFileType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(allowedFileType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(allowedFileType);
        }

        // GET: AllowedFileTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AllowedFileType allowedFileType = db.AllowedFileTypes.Find(id);
            if (allowedFileType == null)
            {
                return HttpNotFound();
            }
            return View(allowedFileType);
        }

        // POST: AllowedFileTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AllowedFileType allowedFileType = db.AllowedFileTypes.Find(id);
            db.AllowedFileTypes.Remove(allowedFileType);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
