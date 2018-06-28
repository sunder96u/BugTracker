using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Action_Filters;
using BugTracker.Models;

namespace BugTracker.Controllers
{
    [AdminAuthorization]
    [RequireHttps]
    public class ProjectStatsesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProjectStatses
        public ActionResult Index()
        {
            return View(db.ProjectStatuses.ToList());
        }

        // GET: ProjectStatses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Oops5", "Home", null);
            }
            ProjectStats projectStats = db.ProjectStatuses.Find(id);
            if (projectStats == null)
            {
                return RedirectToAction("Oops5", "Home", null);
            }
            return View(projectStats);
        }

        // GET: ProjectStatses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProjectStatses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] ProjectStats projectStats)
        {
            if (ModelState.IsValid)
            {
                db.ProjectStatuses.Add(projectStats);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(projectStats);
        }

        // GET: ProjectStatses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Oops5", "Home", null);
            }
            ProjectStats projectStats = db.ProjectStatuses.Find(id);
            if (projectStats == null)
            {
                return RedirectToAction("Oops5", "Home", null);
            }
            return View(projectStats);
        }

        // POST: ProjectStatses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] ProjectStats projectStats)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectStats).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(projectStats);
        }

        // GET: ProjectStatses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Oops5", "Home", null);
            }
            ProjectStats projectStats = db.ProjectStatuses.Find(id);
            if (projectStats == null)
            {
                return RedirectToAction("Oops5", "Home", null);
            }
            return View(projectStats);
        }

        // POST: ProjectStatses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectStats projectStats = db.ProjectStatuses.Find(id);
            db.ProjectStatuses.Remove(projectStats);
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
