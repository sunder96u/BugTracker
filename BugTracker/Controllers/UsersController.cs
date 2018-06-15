using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Helpers;
using BugTracker.Models;
using BugTracker.Views;

namespace BugTracker.Controllers
{
    [Authorize]
    [RequireHttps]
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper rolesHelper = new UserRolesHelper();
        private ProjectHelper projectHelper = new ProjectHelper();

        // GET: Users
        public ActionResult Index()
        {
    

            return View(db.Users.ToList());
        }
        // GET: Users
        public ActionResult AdminIndex()
        {


            return View(db.Users.ToList());
        }
        // GET: Users
        public ActionResult PMIndex()
        {


            return View(db.Users.ToList());
        }
        // GET: Users
        public ActionResult DevIndex()
        {


            return View(db.Users.ToList());
        }
        // GET: Users
        public ActionResult SubIndex()
        {


            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }
        [Authorize(Roles = "Admin")]
        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,,Email,PhoneNumber")] ApplicationUser applicationUser, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (ImageUploadValidator.IsWebFriendlyImage(image))
                {
                    var fileName = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/Images/"), fileName));
                    applicationUser.Picture = "/Images/" + fileName;
                }
                

                db.Users.Add(applicationUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(applicationUser);
        }
        [Authorize(Roles = "Admin,Project Manager")]
        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            // Setup A selectlist for the users to select
            var occupiedRoles = rolesHelper.ListUserRoles(id).FirstOrDefault();
            ViewBag.Roles = new SelectList(db.Roles, "Name", "Name", occupiedRoles);

            var myProjectIds = new List<int>();
            var myProjects = projectHelper.ListUserProjects(id);
            foreach (var project in myProjects)
                myProjectIds.Add(project.Id);
            ViewBag.Projects = new MultiSelectList(db.Projects, "Id", "Name", myProjectIds);

            return View(applicationUser);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Email,PhoneNumber,Picture,DisplayName")] ApplicationUser applicationUser, HttpPostedFileBase image, string Roles, List<int> Projects)
        {
            if (ModelState.IsValid)
            {
                if (ImageUploadValidator.IsWebFriendlyImage(image))
                {
                    var fileName = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/Images/"), fileName));
                    applicationUser.Picture = "/Images/" + fileName;
                }

                applicationUser.UserName = applicationUser.Email;
                //Assign the user to the selected role
                //Determine if the user currently occupies a role, and if so remove them form it
                foreach (var role in rolesHelper.ListUserRoles(applicationUser.Id))
                {
                    rolesHelper.RemoveUserFromRole(applicationUser.Id, role);
                }
                if(Roles != null)
                    rolesHelper.AddUserToRole(applicationUser.Id, Roles);
                //Add user to the new selected role
                foreach (var proj in projectHelper.ListUserProjects(applicationUser.Id))
                {
                    projectHelper.RemoveUserFromProject(applicationUser.Id, proj.Id);
                }
                if (Projects != null)
                    foreach (var projectId in Projects)
                    projectHelper.AddUserToProject(applicationUser.Id, projectId);

                //db.Entry(applicationUser).State = EntityState.Unchanged;
                db.Users.Attach(applicationUser);
                db.Entry(applicationUser).Property(p => p.FirstName).IsModified = true;
                db.Entry(applicationUser).Property(p => p.LastName).IsModified = true;
                db.Entry(applicationUser).Property(p => p.Email).IsModified = true;
                db.Entry(applicationUser).Property(p => p.PhoneNumber).IsModified = true;
                db.Entry(applicationUser).Property(p => p.DisplayName).IsModified = true;
                db.Entry(applicationUser).Property(p => p.UserName).IsModified = true;
                db.Entry(applicationUser).Property(p => p.Picture).IsModified = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }
        [Authorize]
        // GET: Users/Edit/5
        public ActionResult NameChange(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            return View(applicationUser);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NameChange([Bind(Include = "Id,FirstName,LastName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {

                //db.Entry(applicationUser).State = EntityState.Unchanged;
                applicationUser.UserName = "foo";
                db.Users.Attach(applicationUser);
                db.Entry(applicationUser).Property(p => p.FirstName).IsModified = true;
                db.Entry(applicationUser).Property(p => p.LastName).IsModified = true;
                db.Entry(applicationUser).Property(p => p.UserName).IsModified = false;



                db.SaveChanges();
                return RedirectToAction("AccountProfile", "Account");
            }
            return View(applicationUser);
        }

        [Authorize]
        // GET: Users/Edit/5
        public ActionResult PictureChange(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            return View(applicationUser);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PictureChange([Bind(Include = "Id,Picture")] ApplicationUser applicationUser, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (ImageUploadValidator.IsWebFriendlyImage(image))
                {
                    var fileName = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/Images/"), fileName));
                    applicationUser.Picture = "/Images/" + fileName;
                }


                //db.Entry(applicationUser).State = EntityState.Unchanged;
                applicationUser.UserName = "foo";
                db.Users.Attach(applicationUser);
                db.Entry(applicationUser).Property(p => p.UserName).IsModified = false;
                db.Entry(applicationUser).Property(p => p.Picture).IsModified = true;



                db.SaveChanges();
                return RedirectToAction("AccountProfile", "Account");
            }
            return View(applicationUser);
        }
        [Authorize(Roles = "Admin")]
        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            db.Users.Remove(applicationUser);
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
