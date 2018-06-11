using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Helpers;
using BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    [Authorize]
    [RequireHttps]
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper rolesHelper = new UserRolesHelper();
        private ProjectHelper projectHelper = new ProjectHelper();

        // GET: Projects
        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        //Get:
        public ActionResult OpenProjects()
        {
            var userId = User.Identity.GetUserId();
            var myProjectsIds = new List<int>();
            var myProjects = projectHelper.ListUserProjects(userId);

            if (User.IsInRole("Admin"))
            {
                myProjects = db.Projects.ToList();
            }
            if (User.IsInRole("Project Manager"))
            {
                foreach (var project in myProjects)
                {
                    var projId = project.Id;
                    myProjects = db.Projects.Where(p => p.Id == projId).ToList();
                }
            }
            if (User.IsInRole("Developer"))
            {
                foreach (var project in myProjects)
                {
                    var projId = project.Id;
                    myProjects = db.Projects.Where(p => p.Id == projId).ToList();
                }
            }
            if (User.IsInRole("Submitter"))
            {
                foreach (var project in myProjects)
                {
                    var projId = project.Id;
                    myProjects = db.Projects.Where(p => p.Id == projId).ToList();
                }
            }


            return View(myProjects.Where(s => s.Status == "Open"));
        }

        //Get:
        public ActionResult ClosedProjects()
        {
            var userId = User.Identity.GetUserId();
            var myProjectsIds = new List<int>();
            var myProjects = projectHelper.ListUserProjects(userId);

            if (User.IsInRole("Admin"))
            {
                myProjects = db.Projects.ToList();
            }
            if (User.IsInRole("Project Manager"))
            {
                foreach (var project in myProjects)
                {
                    var projId = project.Id;
                    myProjects = db.Projects.Where(p => p.Id == projId).ToList();
                }
            }
            if (User.IsInRole("Developer"))
            {
                foreach (var project in myProjects)
                {
                    var projId = project.Id;
                    myProjects = db.Projects.Where(p => p.Id == projId).ToList();
                }
            }
            if (User.IsInRole("Submitter"))
            {
                foreach (var project in myProjects)
                {
                    var projId = project.Id;
                    myProjects = db.Projects.Where(p => p.Id == projId).ToList();
                }
            }

            return View(myProjects.Where(s => s.Status == "Closed").ToList());
        }

        //Get:
        public ActionResult AllProjects()
        {
            return View(db.Projects.ToList());
        }


        //Get:
        public ActionResult AssignUser(int? id)
        {
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            // 1. Setup a MultiSelectList to display all the Projects in our system
            ViewBag.Projects = new MultiSelectList(db.Projects, "Id", "Name");

            // 2. Set up a Select List to display all the users in the system
            var ProjectManagers = rolesHelper.UsersInRole("Project Manager");
            ViewBag.PMs = new MultiSelectList(ProjectManagers, "Id", "DisplayName");
            var Developer = rolesHelper.UsersInRole("Developer");
            ViewBag.Devs = new MultiSelectList(Developer, "Id", "DisplayName");
            var Submitter = rolesHelper.UsersInRole("Submitter");
            ViewBag.Subs = new MultiSelectList(Submitter, "Id", "DisplayName");


            return View(project);
        }

        //Post:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignUser([Bind(Include = "Id,Name")] List<int> Projects, string Devs, string Subs)
        {
            if (ModelState.IsValid)
            {
                //Assign the user to the selected role
                //Determine if the user currently occupies a project, and if so remove them from it

                foreach (var project in projectHelper.ListUserProjects(Devs))
                {
                    projectHelper.RemoveUserFromProject(Devs, project.Id);
                }
                foreach (var project in projectHelper.ListUserProjects(Subs))
                {
                    projectHelper.RemoveUserFromProject(Subs, project.Id);
                }

                foreach (var projectId in Projects)
                    projectHelper.AddUserToProject(Devs, projectId);
                foreach (var projectId in Projects)
                    projectHelper.AddUserToProject(Subs, projectId);

                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [Authorize(Roles = ("Admin"))]
        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }
        [Authorize(Roles = ("Admin"))]
        // GET: Projects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Status")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }
        [Authorize(Roles = ("Admin,Project Manager"))]
        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Status")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        [Authorize(Roles = ("Admin,Project Manager"))]
        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
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
