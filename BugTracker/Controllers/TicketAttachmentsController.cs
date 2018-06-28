using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Action_Filters;
using BugTracker.Extension_Methods;
using BugTracker.Models;
using BugTracker.Views;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    [Authorize]
    [RequireHttps]
    public class TicketAttachmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketAttachments
        [AdminAuthorization]
        public ActionResult Index()
        {
            var ticketAttachments = db.TicketAttachments.Include(t => t.Ticket).Include(t => t.User);
            return View(ticketAttachments.ToList());
        }

        // GET: TicketAttachments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Oops5", "Home", null);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return RedirectToAction("Oops5", "Home", null);
            }
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Create
        public ActionResult Create()
        {
            //ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title");
            //ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: TicketAttachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TicketId,FilePath,Description,Created,UserId")] TicketAttachment ticketAttachment, HttpPostedFileBase image)
        {

            var oldTicketAttachment = db.TicketAttachments.AsNoTracking().FirstOrDefault(t => t.Id == ticketAttachment.TicketId);

            if (ModelState.IsValid)
            {

                if (FileUploadValidator.IsWebFriendlyFile(image))
                {
                    var fileName = Path.GetFileName(image.FileName).Replace(' ','_');
                    image.SaveAs(Path.Combine(Server.MapPath("~/FilesUploaded/"), fileName));
                    ticketAttachment.FilePath = "/FilesUploaded/" + fileName;
                }

                ticketAttachment.Created = DateTimeOffset.Now;
                ticketAttachment.UserId = User.Identity.GetUserId();

                ticketAttachment.User = db.Users.Find(ticketAttachment.UserId);
                ticketAttachment.Ticket = db.Tickets.Find(ticketAttachment.TicketId);

                db.TicketAttachments.Add(ticketAttachment);
                db.SaveChanges();

                ticketAttachment.AttachmentAdded( oldTicketAttachment);


                var id = db.Tickets.Find(ticketAttachment.TicketId).Id;
                return RedirectToAction("Details", "Tickets", new { Id = id });
               
            }
            //ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttachment.TicketId);
            //ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserId);
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Edit/5
        [AdminAuthorization]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Oops5", "Home", null);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return RedirectToAction("Oops5", "Home", null);
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttachment.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserId);
            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TicketId,FilePath,Description,Created,Updated,UserId")] TicketAttachment ticketAttachment)
        {

            var oldTicketAttachment = db.TicketAttachments.AsNoTracking().FirstOrDefault(t => t.Id == ticketAttachment.Id);

            if (ModelState.IsValid)
            {

                ticketAttachment.UserId = User.Identity.GetUserId();
                db.TicketAttachments.Add(ticketAttachment);

                db.Entry(ticketAttachment).State = EntityState.Modified;
                db.SaveChanges();

                ticketAttachment.AttachmentEditted(oldTicketAttachment);

                return RedirectToAction("Index");
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttachment.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserId);
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Delete/5
        [AdminAuthorization]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Oops5", "Home", null);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return RedirectToAction("Oops5", "Home", null);
            }
            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            db.TicketAttachments.Remove(ticketAttachment);
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
