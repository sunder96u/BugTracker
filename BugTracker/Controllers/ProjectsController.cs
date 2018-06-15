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
        private TicketHelper ticketHelper = new TicketHelper();

        // GET: Projects
        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        //Get:
        public ActionResult MyProjects()
        {
            var userId = User.Identity.GetUserId();
            var myProjects = projectHelper.ListUserProjects(userId);

            if (User.IsInRole("Admin"))
            {
                myProjects = db.Projects.ToList();
            }

            return View(myProjects);

        }

        //Get:
        public ActionResult AllProjects()
        {
            return View(db.Projects.ToList());
        }

        // GET: MyTickets
        public ActionResult MyTickets()
        {
            {
                var userId = User.Identity.GetUserId();
                var myProject = projectHelper.ListUserProjects(userId);
                var myTickets = ticketHelper.GetProjectTickets(userId).ToList();

                if (User.IsInRole("Admin"))
                {
                    myProject = db.Projects.ToList();
                }

                return View(myProject);
            }
        }

        // GET: MyProjectTickets
        public ActionResult MyProjectTickets()
        {
            {
                var userId = User.Identity.GetUserId();
                var myProject = projectHelper.ListUserProjects(userId);

                if (User.IsInRole("Admin"))
                {
                    myProject = db.Projects.ToList();
                }

                return View(myProject);
            }
        }

        //Get:
        public ActionResult AssignUser(int id)
        {
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            // 1. Setup a MultiSelectList to display all the Projects in our system
            var UsersOnProject = projectHelper.UsersOnProject(id);
            var projDevs = new List<string>();
            var projSubs = new List<string>();

            foreach (var devUser in UsersOnProject)
            {
                if (rolesHelper.IsUserInRole(devUser.Id, "Developer"))
                    projDevs.Add(devUser.Id);
            }
            foreach (var subUser in UsersOnProject)
            {
                if (rolesHelper.IsUserInRole(subUser.Id, "Developer"))
                    projDevs.Add(subUser.Id);
            }

            var Developer = rolesHelper.UsersInRole("Developer");
            ViewBag.Devs = new MultiSelectList(Developer, "Id", "DisplayName", projDevs);


            var Submitter = rolesHelper.UsersInRole("Submitter");
            ViewBag.Subs = new MultiSelectList(Submitter, "Id", "DisplayName", projSubs);


            return View(project);
        }

        //Post:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignUser([Bind(Include = "Id,Name")] Project project, /*List<int> Projects,*/ List<string> Devs, List<string> Subs)
        {
            if (ModelState.IsValid)
            {
                //Assign the user to the selected role
                //Determine if the user currently occupies a project, and if so remove them from it
                var UsersOnProject = projectHelper.UsersOnProject(project.Id).ToList();

                foreach (var user in UsersOnProject)
                {
                    if(!rolesHelper.IsUserInRole(user.Id,"Project Manager"))
                    projectHelper.RemoveUserFromProject(user.Id, project.Id);
                }

                foreach (var devId in Devs)
                {
                    projectHelper.AddUserToProject(devId, project.Id);
                }

                foreach (var subId in Subs)
                {
                    projectHelper.AddUserToProject(subId, project.Id);
                }

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
        [Authorize(Roles = ("Admin,Project Manager"))]
        // GET: Projects/Create
        public ActionResult Create()
        {
            ViewBag.Status = new SelectList(db.ProjectStatuses, "Id", "Name");
            return View();
        }


        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
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

            // create a list option that will only show Open or Closed

            ViewBag.Status = new SelectList(db.ProjectStatuses, "Id", "Name");

            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
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
            return RedirectToAction("Index", "Home");
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
