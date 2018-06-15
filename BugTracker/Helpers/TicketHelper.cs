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

    }
}