using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;

namespace BugTracker.ViewModels
{
    public class DashboardVM
    {
        //Creating the Data for Badges or Labels
        public TicketDashboardData TicketData { get; set; }
        public ProjectDashboardData ProjectData { get; set; }
        public TableDashboardData TableData { get; set; }

        public DashboardVM()
        {
            TableData = new TableDashboardData();
            TicketData = new TicketDashboardData();
            ProjectData = new ProjectDashboardData();
        }

    }

    public class TableDashboardData
    {
        public List<Project> Projects { get; set; }
        public List<Ticket> Tickets { get; set; }
        public List<TicketAttachment> TicketAttachments { get; set; }
        public List<TicketComment> TicketsComments { get; set; }
        public List<TicketHistory> TicketsHistories { get; set; }
        public List<TicketNotification> TicketNotifications { get; set; }

        public TableDashboardData()
        {
            this.Tickets = new List<Ticket>();
            this.Projects = new List<Project>();
            this.TicketAttachments = new List<TicketAttachment>();
            this.TicketsComments = new List<TicketComment>();
            this.TicketsHistories = new List<TicketHistory>();
            this.TicketNotifications = new List<TicketNotification>();
        }

    }

    public class TicketDashboardData
    {
        public int TicketCount { get; set; }
        public int UnassignedTicketCount { get; set; }
        public int NotinProgressCount { get; set; }
        public int InProgressCount { get; set; }
        public int OnHoldCount { get; set; }
        public int ResolvedCount { get; set; }
        public int ClosedCount { get; set; }
        public int TicketCommentCount { get; set; }
        public int TicketNotificationsCount { get; set; }
    }

    public class ProjectDashboardData
    {
        public int ProjectCount { get; set; }
        public int OpenProjectCount { get; set; }
        public int ClosedProjectCount { get; set; }
        public int OnHoldProjectCount { get; set; }
        public int WaitingOnApprovalProjectCount { get; set; }
    }
}