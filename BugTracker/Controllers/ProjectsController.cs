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
                if (rolesHelper.IsUserInRole(subUser.Id, "Submitter"))
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

                return RedirectToAction("Details", "Projects", new { Id = project.Id });
            }
            return View();
        }


        // GET: Admin
        public ActionResult AssignPMs(int id)
        {
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            // 1. Setup a MultiSelectList to display all the Projects in our system
            ViewBag.Projects = new MultiSelectList(db.Projects, "Id", "Name");



            // 2. Set up a Select List to display all the PM's in the system
            var ProjectManagers = rolesHelper.UsersInRole("Project Manager");
            ViewBag.PMs = new MultiSelectList(ProjectManagers, "Id", "DisplayName");

            return View();
        }


        //Post:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignPMs([Bind(Include = "Id,ProjectId")] Project project, List<string> PMs)
        {
            if (ModelState.IsValid)
            {
                //Assign the user to the selected role
                //Determine if the user currently occupies a role, and if so remove them form it
                var UsersOnProject = projectHelper.UsersOnProject(project.Id).ToList();

                foreach (var user in UsersOnProject)
                {
                    if (!rolesHelper.IsUserInRole(user.Id, "Submitter") && !rolesHelper.IsUserInRole(user.Id, "Developer"))
                        projectHelper.RemoveUserFromProject(user.Id, project.Id);
                }

                foreach (var PM in PMs)
                {
                    projectHelper.AddUserToProject(PM, project.Id);
                }

                return RedirectToAction("Details", "Projects", new { Id = project.Id });

            }
            return View();
        }

        //Get:
        public ActionResult AssignSubmitter(int id)
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
            foreach (var subUser in UsersOnProject)
            {
                if (rolesHelper.IsUserInRole(subUser.Id, "Submitter"))
                    projDevs.Add(subUser.Id);
            }

            var Submitter = rolesHelper.UsersInRole("Submitter");
            ViewBag.Subs = new MultiSelectList(Submitter, "Id", "DisplayName", projSubs);


            return View(project);
        }

        //Post:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignSubmitter([Bind(Include = "Id,Name")] Project project, List<string> Subs)
        {
            if (ModelState.IsValid)
            {
                //Assign the user to the selected role
                //Determine if the user currently occupies a project, and if so remove them from it
                var UsersOnProject = projectHelper.UsersOnProject(project.Id).ToList();

                foreach (var user in UsersOnProject)
                {
                    if (!rolesHelper.IsUserInRole(user.Id, "Project Manager") && !rolesHelper.IsUserInRole(user.Id, "Developer"))
                        projectHelper.RemoveUserFromProject(user.Id, project.Id);
                }

                foreach (var subId in Subs)
                {
                    projectHelper.AddUserToProject(subId, project.Id);
                }

                return RedirectToAction("Details", "Projects", new { Id = project.Id });
            }
            return View();
        }

        //Get:
        public ActionResult AssignDeveloper(int id)
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

            var Developer = rolesHelper.UsersInRole("Developer");
            ViewBag.Devs = new MultiSelectList(Developer, "Id", "DisplayName", projDevs);


            return View(project);
        }

        //Post:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignDeveloper([Bind(Include = "Id,Name")] Project project, List<string> Devs)
        {
            if (ModelState.IsValid)
            {
                //Assign the user to the selected role
                //Determine if the user currently occupies a project, and if so remove them from it
                var UsersOnProject = projectHelper.UsersOnProject(project.Id).ToList();

                foreach (var user in UsersOnProject)
                {
                    if (!rolesHelper.IsUserInRole(user.Id, "Project Manager") && !rolesHelper.IsUserInRole(user.Id, "Submitter"))
                        projectHelper.RemoveUserFromProject(user.Id, project.Id);
                }

                foreach (var devId in Devs)
                {
                    projectHelper.AddUserToProject(devId, project.Id);
                }

                return RedirectToAction("Details", "Projects", new { Id = project.Id });
            }
            return View();
        }



        // GET: Projects/Details/5
        public ActionResult Details(int id)
        {
            var UsersOnProject = projectHelper.UsersOnProject(id);
            var projDevs = new List<ApplicationUser>();
            var projSubs = new List<ApplicationUser>();
            var projPMs = new List<ApplicationUser>();

            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            foreach (var PmUser in UsersOnProject)
            {
                if (rolesHelper.IsUserInRole(PmUser.Id, "Project Manager"))
                    projPMs.Add(PmUser);
            }
            foreach (var devUser in UsersOnProject)
            {
                if (rolesHelper.IsUserInRole(devUser.Id, "Developer"))
                    projDevs.Add(devUser);
            }
            foreach (var subUser in UsersOnProject)
            {
                if (rolesHelper.IsUserInRole(subUser.Id, "Submitter"))
                    projSubs.Add(subUser);
            }
            var ProjectManager = projPMs.ToList();
            ViewBag.Pms = new SelectList(ProjectManager, "Id", "DisplayName", projPMs);


            var Developer = projDevs.ToList();
            ViewBag.Devs = new SelectList(Developer, "Id", "DisplayName", projDevs);


            var Submitter = projSubs.ToList();
            ViewBag.Subs = new SelectList(Submitter, "Id", "DisplayName", projSubs);

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
