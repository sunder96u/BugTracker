using BugTracker.Helpers;
using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [Authorize(Roles = "Admin")]
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
        public ActionResult AssignPMs(List<int> Projects, string PMs)
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
        
    }
}
