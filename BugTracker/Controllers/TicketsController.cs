using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BugTracker.Action_Filters;
using BugTracker.Extension_Methods;
using BugTracker.Helpers;
using BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    [Authorize]
    [RequireHttps]
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper rolesHelper = new UserRolesHelper();
        private ProjectHelper projectHelper = new ProjectHelper();
        private TicketHelper ticketHelper = new TicketHelper();


        // GET: Tickets
        public ActionResult Index()
        {
            {
                return View(db.Tickets.ToList());
            }
        }


        // GET: MyTickets
        public ActionResult MyTickets()
        {
            {
                var userId = User.Identity.GetUserId();
                var myProject = projectHelper.ListUserProjects(userId);
                var myTickets = ticketHelper.GetProjectTickets(userId).ToList();

                return View(myTickets);
            }
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int id)
        {

            var projPMs = new List<ApplicationUser>();


            Ticket Ticket = db.Tickets.Find(id);
            var projid = Ticket.ProjectId;
            var UsersOnProject = projectHelper.UsersOnProject(projid).ToList();

            if (Ticket == null)
            {
                return HttpNotFound();
            }

            foreach (var PmUser in UsersOnProject)
            {
                if (rolesHelper.IsUserInRole(PmUser.Id, "Project Manager"))
                    projPMs.Add(PmUser);
            }

            var ProjectManager = projPMs.ToList();
            ViewBag.Pms = new SelectList(ProjectManager, "Id", "DisplayName", projPMs);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses.Where(t => t.Name != "Unassigned" && t.Name != "Closed"), "Id", "Name", Ticket.TicketStatusId);

            return View(Ticket);
        }
        [Authorize(Roles = "Submitter")]
        // GET: Tickets/Create
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            var proj = projectHelper.ListUserProjects(userId);
            ViewBag.ProjectId = new SelectList(proj, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");

            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Created,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.OwnerUserId = User.Identity.GetUserId();
                ticket.TicketStatusId = 1;
                ticket.Created = DateTimeOffset.Now;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }

            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);

            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [TicketAuthorization]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Updated,ProjectId,TicketTypeId,TicketPriorityId")] Ticket ticket)
        {
            var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

            if (ModelState.IsValid)
            {
                ticket.Updated = DateTimeOffset.Now;
                ticket.TicketStatusId = 1;
                db.Tickets.Attach(ticket);
                db.Entry(ticket).Property(p => p.TicketStatusId).IsModified = false;
                db.Entry(ticket).Property(p => p.Title).IsModified = true;
                db.Entry(ticket).Property(p => p.Description).IsModified = true;
                db.Entry(ticket).Property(p => p.Updated).IsModified = true;
                db.Entry(ticket).Property(p => p.ProjectId).IsModified = true;
                db.Entry(ticket).Property(p => p.TicketTypeId).IsModified = true;
                db.Entry(ticket).Property(p => p.TicketPriorityId).IsModified = true;

                db.SaveChanges();

                ticket.RecordChanges(oldTicket);

                return RedirectToAction("Index");
            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStatus([Bind(Include = "Id,TicketStatusId,Updated,Title,Description")] Ticket ticket)
        {

            var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

            if (ModelState.IsValid)
            {
                ticket.Updated = DateTimeOffset.Now;
                ticket.Title = "foo";
                ticket.Description = "foo";
                db.Tickets.Attach(ticket);
                db.Entry(ticket).Property(p => p.Updated).IsModified = true;
                db.Entry(ticket).Property(p => p.TicketStatusId).IsModified = true;
                db.Entry(ticket).Property(p => p.Description).IsModified = false;
                db.Entry(ticket).Property(p => p.Title).IsModified = false;
                db.SaveChanges();

                ticket.StatusChanges(oldTicket);

                return RedirectToAction("Details", "Tickets", new { Id = ticket.Id });
            }
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);

            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public ActionResult AssignDev(int id)
        {

            //var UsersOnProject = db.Users.ToList();
            var projDevs = new List<ApplicationUser>();

            Ticket ticket = db.Tickets.Find(id);

            var projId = ticket.ProjectId;
            var UsersOnProject = projectHelper.UsersOnProject(projId).ToList();

            if (ticket == null)
            {
                return HttpNotFound();
            }
            foreach (var devUser in UsersOnProject)
            {
                if (rolesHelper.IsUserInRole(devUser.Id, "Developer"))
                    projDevs.Add(devUser);
            }
            var Developers = projDevs.ToList();
            var Developer = rolesHelper.UsersInRole("Developer");
            ViewBag.Devs = new SelectList(Developers, "Id", "DisplayName", projDevs);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AssignDev([Bind(Include = "Id,Title,Description,TicketTitleId,Created,TicketTypeId,TicketStatusId,TicketPriorityId,ProjectId")] Ticket ticket, string Devs)
        {
            var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

            if (ModelState.IsValid)
            {
                ticket.Title = "foo";
                ticket.Description = "foo";
                if (ticket.TicketStatusId == 1)
                {
                    ticket.TicketStatusId = 2;
                }
                else
                {
                    ticket.TicketStatusId = ticket.TicketStatusId;
                }
                ticket.AssignedToUserId = Devs;
                ticket.Updated = DateTimeOffset.Now;
                db.Tickets.Attach(ticket);
                db.Entry(ticket).Property(a => a.TicketStatusId).IsModified = true;
                db.Entry(ticket).Property(a => a.AssignedToUserId).IsModified = true;
                db.Entry(ticket).Property(a => a.Title).IsModified = false;
                db.Entry(ticket).Property(a => a.Description).IsModified = false;
                db.Entry(ticket).Property(a => a.Created).IsModified = false;
                db.Entry(ticket).Property(a => a.TicketTypeId).IsModified = false;
                db.Entry(ticket).Property(a => a.Title).IsModified = false;
                db.Entry(ticket).Property(a => a.TicketStatusId).IsModified = true;
                db.Entry(ticket).Property(a => a.TicketPriorityId).IsModified = false;
                db.Entry(ticket).Property(a => a.ProjectId).IsModified = false;

                ticket.DevChanges(oldTicket);
                await ticket.DeveloperAssignemt(oldTicket);

                db.SaveChanges();
                return RedirectToAction("Details", "Tickets", new { Id = ticket.Id });
            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "DisplayName", ticket.AssignedToUserId);

            return View(ticket);
        }
        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
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
