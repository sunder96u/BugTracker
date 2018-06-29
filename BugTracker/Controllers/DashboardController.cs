using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using BugTracker.ViewModels;
using BugTracker.Helpers;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private TicketHelper ticketHelper = new TicketHelper();
        private ProjectHelper projhelper = new ProjectHelper();

        // GET: Dashboard
        public ActionResult Index()
        {
            //create the data for the dashboard using the ViewModel
            var data = new DashboardVM();
            var myTickets = ticketHelper.GetMyTicketsByRole(User.Identity.GetUserId());
            var myProject = projhelper.myProjects(User.Identity.GetUserId());
            var myTicketHistory = ticketHelper.myTicketHistory(User.Identity.GetUserId());
            var myTicketAttachments = ticketHelper.myTicketAttachments(User.Identity.GetUserId());
            var myTicketNotifications = ticketHelper.myTicketNotifications(User.Identity.GetUserId());
            var myTicketComments = ticketHelper.myTicketComments(User.Identity.GetUserId());


            var projman = new List<ApplicationUser>();
            


            //Load up all the TableData
            data.TableData.Projects = db.Projects.OrderByDescending(t => t.Id).ToList();
            data.TableData.Tickets = myTickets.OrderByDescending(t => t.Created).ToList();
            data.TableData.TicketAttachments = myTicketAttachments.OrderByDescending(t => t.Created).ToList();
            data.TableData.TicketNotifications = myTicketNotifications.OrderByDescending(t => t.Created).ToList();
            data.TableData.TicketsComments = myTicketComments.OrderByDescending(t => t.Created).ToList();
            data.TableData.TicketsHistories = myTicketHistory.OrderByDescending(t => t.Changed).ToList();
            data.TableData.ProjUser = projman.ToList();

            //data.TableData.Tickets = db.Tickets.Where(t => t.Tickets == myTickets).OrderByDescending(t => t.Created).ToList();

            //Load up all the Ticket Dashboard Data

            data.TicketData.TicketCount = myTickets.Count();
            data.TicketData.UnassignedTicketCount = myTickets.Where(c => c.TicketStatus.Name == "Unassigned").Count();
            data.TicketData.NotinProgressCount = myTickets.Where(c => c.TicketStatus.Name == "Not in Progress").Count();
            data.TicketData.InProgressCount = myTickets.Where(c => c.TicketStatus.Name == "In Progress").Count();
            data.TicketData.OnHoldCount = myTickets.Where(c => c.TicketStatus.Name == "On Hold").Count();
            data.TicketData.ResolvedCount = myTickets.Where(c => c.TicketStatus.Name == "Resolved").Count();
            data.TicketData.ClosedCount = myTickets.Where(c => c.TicketStatus.Name == "Closed").Count();
            data.TicketData.TicketNotificationsCount = myTicketNotifications.Count();
            data.TicketData.TicketCommentCount = myTicketComments.Count();
            data.TicketData.TicketHistoryCount = myTicketHistory.Count();
            data.TicketData.TicketAttachmentCount = myTicketAttachments.Count();
            data.TicketData.ImmediateCount = myTickets.Where(c => c.TicketPriority.Name == "Immediate").Count();
            data.TicketData.HighCount = myTickets.Where(c => c.TicketPriority.Name == "High").Count();
            data.TicketData.MediumCount = myTickets.Where(c => c.TicketPriority.Name == "Medium").Count();
            data.TicketData.LowCount = myTickets.Where(c => c.TicketPriority.Name == "Low").Count();
            data.TicketData.NoPriorityCount = myTickets.Where(c => c.TicketPriority.Name == "No Priority").Count();
            data.TicketData.IgnoreCount = myTickets.Where(c => c.TicketPriority.Name == "Ignore").Count();


            //Load up all the Project Dashboard Data

            if(User.IsInRole("Admin"))
            {
                data.ProjectData.ProjectCount = db.Projects.Count();
            }
            if(!User.IsInRole("Admin"))
            {
            data.ProjectData.ProjectCount = myProject.Count();
            }
            data.ProjectData.OpenProjectCount = db.Projects.Where(c => c.ProjectStats.Name == "Open").Count();
            data.ProjectData.OnHoldProjectCount = db.Projects.Where(c => c.ProjectStats.Name == "On Hold").Count();
            data.ProjectData.ClosedProjectCount = db.Projects.Where(c => c.ProjectStats.Name == "Closed").Count();
            data.ProjectData.WaitingOnApprovalProjectCount = db.Projects.Where(c => c.ProjectStats.Name == "Waiting on Approval").Count();

            return View(data);
        }

    }
}