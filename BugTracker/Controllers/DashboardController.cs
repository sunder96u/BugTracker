using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using BugTracker.ViewModels;

namespace BugTracker.Controllers
{
    public class DashboardController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Dashboard
        public ActionResult Index()
        {
            //create the data for the dashboard using the ViewModel
            var data = new DashboardVM();


            //Load up all the TableData
            data.TableData.Projects = db.Projects.OrderByDescending(t => t.Id).Take(5).ToList();
            data.TableData.Tickets = db.Tickets.OrderByDescending(t => t.Created).Take(5).ToList();
            data.TableData.TicketAttachments = db.TicketAttachments.OrderByDescending(t => t.Created).Take(5).ToList();
            data.TableData.TicketNotifications = db.TicketNotifications.OrderByDescending(t => t.Created).Take(5).ToList();
            data.TableData.TicketsComments = db.TicketComments.OrderByDescending(t => t.Created).Take(5).ToList();
            data.TableData.TicketsHistories = db.TicketHistories.OrderByDescending(t => t.Changed).Take(5).ToList();

            //Load up all the Ticket Dashboard Data
            data.TicketData.TicketCount = db.Tickets.Count();
            data.TicketData.UnassignedTicketCount = db.Tickets.Where(c => c.TicketStatus.Name == "Unassigned").Count();
            data.TicketData.NotinProgressCount = db.Tickets.Where(c => c.TicketStatus.Name == "Not in Progress").Count();
            data.TicketData.InProgressCount = db.Tickets.Where(c => c.TicketStatus.Name == "In Progress").Count();
            data.TicketData.OnHoldCount = db.Tickets.Where(c => c.TicketStatus.Name == "On Hold").Count();
            data.TicketData.ResolvedCount = db.Tickets.Where(c => c.TicketStatus.Name == "Resolved").Count();
            data.TicketData.ClosedCount = db.Tickets.Where(c => c.TicketStatus.Name == "Closed").Count();
            data.TicketData.TicketNotificationsCount = db.TicketNotifications.Count();
            data.TicketData.TicketCommentCount = db.TicketComments.Count();



            //Load up all the Project Dashboard Data
            data.ProjectData.ProjectCount = db.Projects.Count();
            data.ProjectData.OpenProjectCount = db.Projects.Where(c => c.ProjectStats.Name == "Open").Count();
            data.ProjectData.OnHoldProjectCount = db.Projects.Where(c => c.ProjectStats.Name == "On Hold").Count();
            data.ProjectData.ClosedProjectCount = db.Projects.Where(c => c.ProjectStats.Name == "Closed").Count();
            data.ProjectData.WaitingOnApprovalProjectCount = db.Projects.Where(c => c.ProjectStats.Name == "Waiting on Approval").Count();

            return View(data);
        }
    }
}