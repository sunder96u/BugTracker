using BugTracker.Action_Filters;
using BugTracker.Helpers;
using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [AdminAuthorization]
    [RequireHttps]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper rolesHelper = new UserRolesHelper();
        private ProjectHelper projectHelper = new ProjectHelper();

        // GET: Admin
        public ActionResult AssignPMs()
        {
            // 1. Setup a MultiSelectList to display all the Projects in our system
            ViewBag.Projects = new MultiSelectList(db.Projects, "Id", "Name");



            // 2. Set up a Select List to display all the PM's in the system
            var ProjectManagers = rolesHelper.UsersInRole("Project Manager");
            ViewBag.PMs = new SelectList(ProjectManagers, "Id", "DisplayName");

            return View();
        }


        //Post:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignPMs(/*[Bind(Include = "Id,ProjectId")]*/List<int> Projects, string PMs)
          {
            if (ModelState.IsValid)
            {
                //Assign the user to the selected role
                //Determine if the user currently occupies a role, and if so remove them form it
                foreach (var project in projectHelper.ListUserProjects(PMs))
                {
                    projectHelper.RemoveUserFromProject(PMs, project.Id);
                }

                foreach (var projectId in Projects)
                    projectHelper.AddUserToProject(PMs, projectId);

                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // GET: Admin
        public ActionResult AssignUsers()
        {
            // 1. Setup a MultiSelectList to display all the Projects in our system
            ViewBag.Projects = new MultiSelectList(db.Projects, "Id", "Name");

            // 2. Set up a Select List to display all the PM's in the system
            var ProjectManagers = rolesHelper.UsersInRole("Project Manager");
            ViewBag.PMs = new MultiSelectList(ProjectManagers, "Id", "DisplayName");
            var Developer = rolesHelper.UsersInRole("Developer");
            ViewBag.Devs = new MultiSelectList(Developer, "Id", "DisplayName");
            var Submitter = rolesHelper.UsersInRole("Submitter");
            ViewBag.Subs = new MultiSelectList(Submitter, "Id", "DisplayName");

            return View();
        }


        //Post:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignUsers(List<int> Projects, string PMs, string Devs, string Subs)
        {
            if (ModelState.IsValid)
            {
                //Assign the user to the selected role
                //Determine if the user currently occupies a role, and if so remove them form it
                foreach (var project in projectHelper.ListUserProjects(PMs))
                {
                    projectHelper.RemoveUserFromProject(PMs, project.Id);
                }
                foreach (var project in projectHelper.ListUserProjects(Devs))
                {
                    projectHelper.RemoveUserFromProject(Devs, project.Id);
                }
                foreach (var project in projectHelper.ListUserProjects(Subs))
                {
                    projectHelper.RemoveUserFromProject(Subs, project.Id);
                }

                foreach (var projectId in Projects)
                    projectHelper.AddUserToProject(PMs, projectId);
                foreach (var projectId in Projects)
                    projectHelper.AddUserToProject(Devs, projectId);
                foreach (var projectId in Projects)
                    projectHelper.AddUserToProject(Subs, projectId);

                return RedirectToAction("Index", "Home");
            }
            return View();
        }

    }
}
