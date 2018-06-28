using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class TicketHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectHelper projectHelper = new ProjectHelper();
        private UserRolesHelper rolesHelper = new UserRolesHelper();

        public ICollection<Ticket> GetProjectTickets(string pmId)
        {
            var myTickets = new List<Ticket>();
            foreach (var project in projectHelper.ListUserProjects(pmId))
            {
                myTickets.AddRange(db.Tickets.Where(t => t.ProjectId == project.Id).ToList());
            }
            return myTickets;
        }


        public ICollection<TicketAttachment> GetProjectAttachments(string pmId)
        {
            var myTicketAttachment = new List<TicketAttachment>();
            foreach (var project in projectHelper.ListUserProjects(pmId))
            {
                myTicketAttachment.AddRange(db.TicketAttachments.Where(t => t.Ticket.ProjectId == project.Id).ToList());
            }
            return myTicketAttachment;
        }

        public ICollection<TicketNotification> GetProjectNotifications(string pmId)
        {
            var myTicketNotifications = new List<TicketNotification>();
            foreach (var project in projectHelper.ListUserProjects(pmId))
            {
                myTicketNotifications.AddRange(db.TicketNotifications.Where(t => t.Ticket.ProjectId == project.Id).ToList());
            }
            return myTicketNotifications;
        }

        public ICollection<TicketComment> GetProjectComments(string pmId)
        {
            var myTicketComments = new List<TicketComment>();
            foreach (var project in projectHelper.ListUserProjects(pmId))
            {
                myTicketComments.AddRange(db.TicketComments.Where(t => t.Ticket.ProjectId == project.Id).ToList());
            }
            return myTicketComments;
        }

        public ICollection<TicketHistory> GetProjectHistory(string pmId)
        {
            var myTicketHistory = new List<TicketHistory>();
            foreach (var project in projectHelper.ListUserProjects(pmId))
            {
                myTicketHistory.AddRange(db.TicketHistories.Where(t => t.Ticket.ProjectId == project.Id).ToList());
            }
            return myTicketHistory;
        }

        public ICollection<Ticket> GetMyTicketsByRole(string userId)
        {
            var myTickets = new List<Ticket>();
            var myRole = rolesHelper.ListUserRoles(userId).FirstOrDefault();
            switch(myRole)
            {
                case ("Admin"):
                    myTickets.AddRange(db.Tickets.ToList());
                    break;
                case ("Project Manager"):
                    myTickets.AddRange(GetProjectTickets(userId));
                    break;
                case ("Developer"):
                    myTickets.AddRange(db.Tickets.Where(t => t.AssignedToUserId == userId).ToList());
                    break;
                case ("Submitter"):
                    myTickets.AddRange(db.Tickets.Where(t => t.OwnerUserId == userId).ToList());
                    break;

            }
            return myTickets;
        }

        public ICollection<Ticket> GetAllTickets(string userId)
        {
            var myTickets = new List<Ticket>();
            var myRole = rolesHelper.ListUserRoles(userId).FirstOrDefault();
            switch (myRole)
            {
                case ("Admin"):
                    myTickets.AddRange(db.Tickets.ToList());
                    break;
                case ("Project Manager"):
                    myTickets.AddRange(db.Tickets.ToList());
                    break;
            }
            return myTickets;
        }

        public ICollection<TicketAttachment> myTicketAttachments(string userId)
        {
            var attach = new List<TicketAttachment>();
            var myRole = rolesHelper.ListUserRoles(userId).FirstOrDefault();
            switch(myRole)
            {
                case ("Admin"):
                    attach.AddRange(db.TicketAttachments.ToList());
                    break;
                case ("Project Manager"):
                    attach.AddRange(GetProjectAttachments(userId));
                    break;
                case ("Developer"):
                    attach.AddRange(db.TicketAttachments.Where(t => t.Ticket.AssignedToUserId == userId).ToList());
                    break;
                case ("Submitter"):
                    attach.AddRange(db.TicketAttachments.Where(t => t.Ticket.OwnerUserId == userId).ToList());
                    break;
            }
            return attach;
        }

        public ICollection<TicketNotification> myTicketNotifications(string userId)
        {
            var attach = new List<TicketNotification>();
            var myRole = rolesHelper.ListUserRoles(userId).FirstOrDefault();
            switch (myRole)
            {
                case ("Admin"):
                    attach.AddRange(db.TicketNotifications.ToList());
                    break;
                case ("Project Manager"):
                    attach.AddRange(GetProjectNotifications(userId));
                    break;
                case ("Developer"):
                    attach.AddRange(db.TicketNotifications.Where(t => t.Ticket.AssignedToUserId == userId).ToList());
                    break;
                case ("Submitter"):
                    attach.AddRange(db.TicketNotifications.Where(t => t.Ticket.OwnerUserId == userId).ToList());
                    break;
            }
            return attach;
        }

        public ICollection<TicketComment> myTicketComments(string userId)
        {
            var attach = new List<TicketComment>();
            var myRole = rolesHelper.ListUserRoles(userId).FirstOrDefault();
            switch (myRole)
            {
                case ("Admin"):
                    attach.AddRange(db.TicketComments.ToList());
                    break;
                case ("Project Manager"):
                    attach.AddRange(GetProjectComments(userId));
                    break;
                case ("Developer"):
                    attach.AddRange(db.TicketComments.Where(t => t.Ticket.AssignedToUserId == userId).ToList());
                    break;
                case ("Submitter"):
                    attach.AddRange(db.TicketComments.Where(t => t.Ticket.OwnerUserId == userId).ToList());
                    break;
            }
            return attach;
        }

        public ICollection<TicketHistory> myTicketHistory(string userId)
        {
            var attach = new List<TicketHistory>();
            var myRole = rolesHelper.ListUserRoles(userId).FirstOrDefault();
            switch (myRole)
            {
                case ("Admin"):
                    attach.AddRange(db.TicketHistories.ToList());
                    break;
                case ("Project Manager"):
                    attach.AddRange(GetProjectHistory(userId));
                    break;
                case ("Developer"):
                    attach.AddRange(db.TicketHistories.Where(t => t.Ticket.AssignedToUserId == userId).ToList());
                    break;
                case ("Submitter"):
                    attach.AddRange(db.TicketHistories.Where(t => t.Ticket.OwnerUserId == userId).ToList());
                    break;
            }
            return attach;
        }
    }
}